﻿using System;
using UnityEngine;


namespace BattlefieldSimulator
{
    /// <summary>
    /// Unit; the object which Users can control;
    /// TODO: 完成对 Unit 类的设计
    /// </summary>
    public class Unit : MonoBehaviour
    {
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
        public int movedistance = 4;
        public int attackdistance = 5;
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
        }

        public void SetStartTile(int q, int r)
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
