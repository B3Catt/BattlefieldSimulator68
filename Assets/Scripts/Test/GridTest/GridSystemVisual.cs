//using System;
//using System.Collections;
//using System.Collections.Generic;
//using BattlefieldSimulator;
//using UnityEditor.Tilemaps;
//using UnityEngine;

//public class GridSystemVisual : MonoBehaviour
//{
//    [SerializeField] private Transform gridSystemVisualSinglePrefab;
//    [SerializeField] private Transform gridSystemVisual;
//    private GridSystemVisualSingle[,] gridSystemVisualSingleArray;
//    public HexGrid hexGrid;

//    public enum GridVisualType
//    {
//        White,
//        Blue,
//        Red,
//        RedSoft,
//        Yellow,
//    }
//    private void Start()
//    {
//        gridSystemVisualSingleArray = new GridSystemVisualSingle[
//            hexGrid.x,
//            hexGrid.z
//        ];
//        foreach (KeyValuePair<Vector2Int, HexTile> pair in hexGrid.tiles)
//        {
//            Vector2Int key = pair.Key;
//            HexTile value = pair.Value;
//            Transform gridSystemVisualSingleTransform =
//                Instantiate(gridSystemVisualSinglePrefab, value.GetWorldPostion() + new Vector3(0, 1.5f, 0), Quaternion.identity, gridSystemVisual);
//            gridSystemVisualSingleArray[key.x, key.y] = gridSystemVisualSingleTransform.GetComponent<GridSystemVisualSingle>();
//        }

//        UnitActionSystem.Instance.OnSelectedUnitChange += UnitActionSystem_OnSelectedUnitChanged;
//        UpdateGridVisual();
//    }

//    public void HideAllSingle()
//    {
//        for (int x = 0; x < hexGrid.x; x++)
//        {
//            for (int z = 0; z < hexGrid.z; z++)
//            {
//                gridSystemVisualSingleArray[x, z].hide();
//            }
//        }
//    }

//    public void ShowTile(int x, int z)
//    {
//        gridSystemVisualSingleArray[x, z].show();
//    }
//    private void UnitActionSystem_OnSelectedUnitChanged(object sender, EventArgs e)
//    {
//        HideAllSingle();
//    }

//    private void UpdateGridVisual()
//    {
//        HideAllSingle();

//        UnitTest selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();
//        BaseAction selectedAction = UnitActionSystem.Instance.GetSelectedAction();
//        GridVisualType gridVisualType=GridVisualType.White;
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
