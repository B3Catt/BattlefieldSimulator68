////using System;
////using System.Collections;
////using System.Collections.Generic;
////using BattlefieldSimulator;
////using UnityEditor.Tilemaps;
////using UnityEngine;

////public class GridSystemVisual : MonoBehaviour
////{
////    [SerializeField] private Transform gridSystemVisualSinglePrefab;
////    [SerializeField] private Transform gridSystemVisual;
////    private GridSystemVisualSingle[,] gridSystemVisualSingleArray;
////    public HexGrid hexGrid;

////    public enum GridVisualType
////    {
////        White,
////        Blue,
////        Red,
////        RedSoft,
////        Yellow,
////    }
////    private void Start()
////    {
////        gridSystemVisualSingleArray = new GridSystemVisualSingle[
////            hexGrid.x,
////            hexGrid.z
////        ];
////        foreach (KeyValuePair<Vector2Int, HexTile> pair in hexGrid.tiles)
////        {
////            Vector2Int key = pair.Key;
////            HexTile value = pair.Value;
////            Transform gridSystemVisualSingleTransform =
////                Instantiate(gridSystemVisualSinglePrefab, value.GetWorldPostion() + new Vector3(0, 1.5f, 0), Quaternion.identity, gridSystemVisual);
////            gridSystemVisualSingleArray[key.x, key.y] = gridSystemVisualSingleTransform.GetComponent<GridSystemVisualSingle>();
////        }

//<<<<<<< HEAD
////        UnitActionSystem.Instance.OnSelectedUnitChange += UnitActionSystem_OnSelectedUnitChanged;
////        UpdateGridVisual();
////    }
//=======
//        UnitActionSystem.Instance.OnSelectedUnitChange += UnitActionSystem_OnSelectedUnitChanged;
//        UnitActionSystem.Instance.OnSelectedActionChanged += UnitActionSystem_OnSelectedActionChanged;
//        UpdateGridVisual();
//    }
//>>>>>>> b0943915cb8330a559681f02d6be1808066e5605

////    public void HideAllSingle()
////    {
////        for (int x = 0; x < hexGrid.x; x++)
////        {
////            for (int z = 0; z < hexGrid.z; z++)
////            {
////                gridSystemVisualSingleArray[x, z].hide();
////            }
////        }
////    }

//<<<<<<< HEAD
////<<<<<<< HEAD
////    public void ShowTile(int x, int z)
////    {
////        gridSystemVisualSingleArray[x, z].show();
////    }
////    private void UnitActionSystem_OnSelectedUnitChanged(object sender, EventArgs e)
////    {
////        HideAllSingle();
////    }
//=======
//    public void ShowTile(int x, int z)
//    {
//        gridSystemVisualSingleArray[x, z].show();
//    }
//    private void UnitActionSystem_OnSelectedUnitChanged(object sender, EventArgs e)
//    {

//        UpdateGridVisual();
//    }
//    private void UnitActionSystem_OnSelectedActionChanged(object sender, EventArgs e)
//    {

//        UpdateGridVisual();
//    }
//>>>>>>> b0943915cb8330a559681f02d6be1808066e5605

////    private void UpdateGridVisual()
////    {
////        HideAllSingle();
////=======
////    public void ShowTile(int x, int z)
////    {
////        gridSystemVisualSingleArray[x, z].show();
////    }
////    private void UnitActionSystem_OnSelectedUnitChanged(object sender, EventArgs e)
////    {
////        //HideAllSingle();
////    }

//<<<<<<< HEAD
////    public void UpdateGridVisual()
////    {
////        HideAllSingle();
////>>>>>>> 35081e962ba6197b0b5ceefb6518836f14ef65f2

////        UnitTest selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();
////        BaseAction selectedAction = UnitActionSystem.Instance.GetSelectedAction();
////        GridVisualType gridVisualType=GridVisualType.White;
////        ShowGridPositionList(selectedAction.GetValidActionGridPositionList(), gridVisualType);
////    }
////    public void ShowGridPositionList(List<HexTile> gridPositionList, GridVisualType gridVisualType)
////    {
////        foreach (HexTile gridPosition in gridPositionList)
////        {
////            gridSystemVisualSingleArray[gridPosition.offsetCoordinate.x, gridPosition.offsetCoordinate.y].show();
////        }
////    }
////}
//=======
//        UnitTest selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();
//        BaseAction selectedAction = UnitActionSystem.Instance.GetSelectedAction();
//        GridVisualType gridVisualType = GridVisualType.White;
//        ShowGridPositionList(selectedAction.GetValidActionGridPositionList(), gridVisualType);
//    }
//    public void ShowGridPositionList(List<HexTile> gridPositionList, GridVisualType gridVisualType)
//    {
//        foreach (HexTile gridPosition in gridPositionList)
//        {
//            gridSystemVisualSingleArray[gridPosition.offsetCoordinate.x, gridPosition.offsetCoordinate.y].show();
//        }
//    }
//}
//>>>>>>> b0943915cb8330a559681f02d6be1808066e5605
