using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Networking;
using System.Threading.Tasks;
using System.Collections;

namespace BattlefieldSimulator
{
    public class UnitTest : MonoBehaviour
    {
        private const int ACTION_POINTS_MAX = 3;
        public static event EventHandler OnAnyActionPointsChanged;
        public static event EventHandler OnAnyUnitSpawned;
        public static event EventHandler OnAnyUnitDead;
        private HexTile currentHexTile;
        private HealthSystem healthSystem;
        private BaseAction[] baseActionArray;
        public HexGrid hexGrid;
        public GridSystemVisual gridSystemVisual;

        [SerializeField] private bool isEnemy;

        [SerializeField] public string unitname;
        [SerializeField] public string unitarmtype;
        [SerializeField] public int movedistance = 4;
        [SerializeField] public int attackdistance = 5;
        [SerializeField] public int attackpower = 40;
        [SerializeField] public string UnitDetails;

        public int x;
        public int z;
        public bool ifselected = false;



        private int actionPoints = ACTION_POINTS_MAX;
        private void Awake()
        {
            hexGrid = FindObjectOfType<HexGrid>();
            gridSystemVisual = FindObjectOfType<GridSystemVisual>();
            baseActionArray = GetComponents<BaseAction>();
            healthSystem = GetComponent<HealthSystem>();
        }
        private void Start()
        {
            SetStartTile(x, z);
            currentHexTile.ifEmpty = false;
            currentHexTile.unitOnIt = this;
            TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
            healthSystem.OnDead += HealthSystem_OnDead;
            OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
            OnAnyUnitSpawned?.Invoke(this, EventArgs.Empty);
        }
        private void Update()
        {
        }

        private void SetStartTile(int x, int z)
        {
            GameObject gridObject = GameObject.Find("grid");
            if (gridObject != null)
            {
                Transform hexTileTransform = gridObject.transform.Find($"Hex C{x},R{z}");
                if (hexTileTransform != null)
                {
                    currentHexTile = hexTileTransform.GetComponent<HexTile>();
                    transform.position = currentHexTile.transform.position + new Vector3(0f, 1f, 0f);
                }
            }

            DataBaseHelper.OpenConnection();
            List<Equip> EquipList = DataBaseHelper.Traverse<Equip>();
            List<ArmType> ArmTypeList = DataBaseHelper.Traverse<ArmType>();
            DataBaseHelper.CloseConnection();
            foreach (Equip pair in EquipList)
            {
                if (pair._name == unitname)
                {
                    unitarmtype = pair._information;
                }
            }
            foreach (ArmType pair in ArmTypeList)
            {
                if (unitarmtype == pair._name)
                {
                    movedistance = pair._speed;
                    attackdistance = pair._attack_distance;
                    attackpower = pair._attack_power;
                    StartCoroutine(UnitGetDetails(unitname));
                }
            }
        }

        private IEnumerator UnitGetDetails(string unitname)
        {
            string url = $"http://127.0.0.1:7687/get_nodes?k={unitname}";

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
            currentHexTile = tile;
            gridSystemVisual.HideAllSingle();
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
            int Damage = damage + attackpower;
            Debug.Log("damage: " + Damage);
            healthSystem.Damage(Damage);
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