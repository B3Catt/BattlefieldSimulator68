//using System.Collections.Generic;
//using UnityEngine;
//using System;

//namespace BattlefieldSimulator
//{
//    public class UnitTest : MonoBehaviour
//    {
//        private const int ACTION_POINTS_MAX = 3;
//        public static event EventHandler OnAnyActionPointsChanged;

//        private HexTile currentHexTile;
//        private BaseAction[] baseActionArray;


//        public int x;
//        public int z;
//        public int movedistance = 4;
//        public bool ifselected = false;
//        public HexGrid hexGrid;
//        public GridSystemVisual gridSystemVisual;


//        private int actionPoints = ACTION_POINTS_MAX;
//        private void Awake()
//        {
//            hexGrid = FindObjectOfType<HexGrid>();
//            gridSystemVisual = FindObjectOfType<GridSystemVisual>();
//            baseActionArray = GetComponents<BaseAction>();
//        }
//        private void Start()
//        {
//            SetStartTile(x, z);
//            TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
//            UnitActionSystem.Instance.OnSelectedUnitChange += UnitActionSystem_OnSelectedUnitChanged;
//        }
//        private void Update()
//        {
//        }

//        private void UnitActionSystem_OnSelectedUnitChanged(object sender, EventArgs e)
//        {
//            gridSystemVisual.UpdateGridVisual();
//        }
//        private void SetStartTile(int x, int z)
//        {
//            GameObject gridObject = GameObject.Find("grid");
//            if (gridObject != null)
//            {
//                Transform hexTileTransform = gridObject.transform.Find($"Hex C{x},R{z}");
//                if (hexTileTransform != null)
//                {
//                    currentHexTile = hexTileTransform.GetComponent<HexTile>();
//                    transform.position = currentHexTile.transform.position + new Vector3(0f, 1f, 0f);
//                }
//            }
//        }

//        private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
//        {

//            actionPoints = ACTION_POINTS_MAX;
//            OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);

//        }

//        public T GetAction<T>() where T : BaseAction
//        {
//            foreach (BaseAction baseAction in baseActionArray)
//            {
//                if (baseAction is T)
//                {
//                    return (T)baseAction;
//                }
//            }
//            return null;
//        }

//        public HexTile GetCurrentHexTile()
//        {
//            return currentHexTile;
//        }
//        public void SetCurrentHexTile(HexTile tile)
//        {
//            currentHexTile = tile;
//            //gridSystemVisual.HideAllSingle();
//        }
//        public BaseAction[] GetBaseActionArray()
//        {
//            return baseActionArray;
//        }

//        public bool TrySpendActionPointsToTakeAction(BaseAction baseAction)
//        {
//            if (CanSpendActionPointsToTakeAction(baseAction))
//            {
//                SpendActionPoints(baseAction.GetActionPointsCost());
//                return true;
//            }
//            else
//            {
//                return false;
//            }
//        }

//        public bool CanSpendActionPointsToTakeAction(BaseAction baseAction)
//        {
//            if (actionPoints >= baseAction.GetActionPointsCost())
//            {
//                return true;
//            }
//            else
//            {
//                return false;
//            }
//        }

//        private void SpendActionPoints(int amount)
//        {
//            actionPoints -= amount;

//            OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
//        }

//        public int GetActionPoints()
//        {
//            return actionPoints;

//        }
//    }
//}

