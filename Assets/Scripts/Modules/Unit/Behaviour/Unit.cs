using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


namespace BattlefieldSimulator
{
    /// <summary>
    /// Unit; the object which Users can control;
    /// TODO: 完成对 Unit 类的设计
    /// </summary>
    public class Unit : MonoBehaviour
    {
        [SerializeField] public string unitname;
        [SerializeField] public string unitarmtype;
        [SerializeField] public int movedistance = 4;
        [SerializeField] public int attackdistance = 5;
        [SerializeField] public int attackpower = 40;
        [SerializeField] public string UnitDetails;

        private const int ACTION_POINTS_MAX = 3;
        public static event EventHandler OnAnyActionPointsChanged;
        public static event EventHandler OnAnyUnitSpawned;
        public static event EventHandler OnAnyUnitDead;

        private HexTile currentHexTile;
        private BaseAction[] baseActionArray;

        private HealthSystem healthSystem;

        [SerializeField] private bool isEnemy;

        public int q;
        public int r;
        public bool ifselected = false;

        public bool isFlight = false;

        private int actionPoints = ACTION_POINTS_MAX;

        private HexGridVisualSystem GridVisualSystem { get => InstanceManager.GridManager.HexGridVisualSystem; }

        private void Awake()
        {
            baseActionArray = GetComponents<BaseAction>();
            healthSystem = GetComponent<HealthSystem>();
        }
        private void Start()
        {
            //设置初始位置
            //SetStartTile(q, r);
            TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
            UnitActionSystem.Instance.OnSelectedUnitChange += UnitActionSystem_OnSelectedUnitChanged;
            healthSystem.OnDead += HealthSystem_OnDead;
            OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
            OnAnyUnitSpawned?.Invoke(this, EventArgs.Empty);
        }
        private void Update()
        {
        }

        private void UnitActionSystem_OnSelectedUnitChanged(object sender, EventArgs e)
        {
            InstanceManager.GridManager.HexGridVisualSystem.UpdateGridVisual();

            if ((sender as UnitActionSystem).IsSelectedUnit(this))
                StartCoroutine(UnitGetDetails(unitname));
        }

        public void Initialize(int q, int r)
        {
            GameObject gridObject = GameObject.Find("Terrain");
            if (gridObject != null)
            {
                Transform hexTileTransform = gridObject.transform.Find($"Tile_ {q}_{r}_{-q - r}");
                if (hexTileTransform != null)
                {
                    currentHexTile = hexTileTransform.GetComponent<HexTile>();
                    transform.position = currentHexTile.transform.position + new Vector3(0f, 1f, 0f);
                    currentHexTile.ifEmpty = false;
                    currentHexTile.unitOnIt = this;
                }
            }
            foreach (Equip pair in InstanceManager.ModelManager.GetData<Equip>().Values)
            {
                if (pair._name == unitname)
                {
                    unitarmtype = pair._information;
                }
            }
            foreach (ArmType pair in InstanceManager.ModelManager.GetData<ArmType>().Values)
            {
                if (unitarmtype == pair._name)
                {
                    movedistance = pair._speed;
                    attackdistance = pair._attack_distance;
                    attackpower = pair._attack_power;
                }
            }
        }

        private IEnumerator UnitGetDetails(string unitname)
        {
            string url = $"http://127.0.0.1:5100/get_nodes?k={unitname}";
            Debug.Log(url);
            string responseText = "1";
            using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
            {
                // 发送请求
                var asyncOperation = webRequest.SendWebRequest();

                // 等待异步操作完成
                while (!asyncOperation.isDone)
                {
                    yield return null; // 等待下一帧
                }

                // 检查是否有错误
                if (webRequest.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError("WebError: " + webRequest.error);
                }
                else
                {
                    responseText = webRequest.downloadHandler.text;
                    Debug.Log("WebResponse: " + responseText);

                }
            }
            UnitDetails = responseText;
            UnitDetailsUI.Instance.UpdateDetailsUI();

        }

        private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
        {
            if ((IsEnemy() && !TurnSystem.Instance.IsPlayerTurn()) ||
                (!IsEnemy() && TurnSystem.Instance.IsPlayerTurn()))
            {
                actionPoints = ACTION_POINTS_MAX;

                OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
            }
        }


        public T GetAction<T>() where T : BaseAction
        {
            foreach (BaseAction baseAction in baseActionArray)
            {
                if (baseAction is T)
                {
                    return (T)baseAction;
                }
            }
            return null;
        }

        public HexTile GetCurrentHexTile()
        {
            return currentHexTile;
        }

        public void SetCurrentHexTile(HexTile tile)
        {
            currentHexTile.ifEmpty = true;
            currentHexTile.unitOnIt = null;

            currentHexTile = tile;
            GridVisualSystem.HideAllSingle();
            
            currentHexTile.ifEmpty = false;
            currentHexTile.unitOnIt = this;
        }
        public BaseAction[] GetBaseActionArray()
        {
            return baseActionArray;
        }

        public bool TrySpendActionPointsToTakeAction(BaseAction baseAction)
        {
            if (CanSpendActionPointsToTakeAction(baseAction))
            {
                SpendActionPoints(baseAction.GetActionPointsCost());
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool CanSpendActionPointsToTakeAction(BaseAction baseAction)
        {
            if (actionPoints >= baseAction.GetActionPointsCost())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void SpendActionPoints(int amount)
        {
            actionPoints -= amount;

            OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
        }

        public int GetActionPoints()
        {
            return actionPoints;
        }

        public bool IsEnemy()
        {
            return isEnemy;
        }
        public Vector3 GetWorldPosition()
        {
            return currentHexTile.transform.position;
        }

        public void Damage(int damage)
        {
            Debug.Log("damage: " + damage);
            healthSystem.Damage(damage);
        }
        private void HealthSystem_OnDead(object sender, EventArgs e)
        {
            currentHexTile.ifEmpty = true;
            currentHexTile.unitOnIt = null;
            Destroy(gameObject);

            OnAnyUnitDead?.Invoke(this, EventArgs.Empty);
        }
        public float GetHealthNormalized()
        {
            return healthSystem.GetHealthNormalized();
        }
    }
}

