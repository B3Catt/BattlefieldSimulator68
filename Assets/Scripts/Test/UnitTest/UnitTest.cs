using System.Collections.Generic;
using UnityEngine;

namespace BattlefieldSimulator
{
    public class UnitTest : MonoBehaviour
    {
        private HexTile currentHexTile;
        private MoveAction moveAction;
        private BaseAction[] baseActionArray;


        public int x;
        public int z;
        //public HexGrid hexGrid;
        public int movedistance = 4;
        public bool ifselected = false;
        //public GridSystemVisual gridSystemVisual;
        private void Awake()
        {
            moveAction = GetComponent<MoveAction>();
            baseActionArray = GetComponents<BaseAction>();
        }
        private void Start()
        {
            //设置初始位置
            // GameObject gridObject = GameObject.Find("grid");
            // if (gridObject != null)
            // {
            //     Transform hexTileTransform = gridObject.transform.Find("Hex C0,R0");
            //     if (hexTileTransform != null)
            //     {
            //         currentHexTile = hexTileTransform.GetComponent<HexTile>();
            //     }
            // }
            SetStartTile(x, z);
        }
        private void Update()
        {
            if (ifselected)
            {
                //foreach (KeyValuePair<Vector2Int, HexTile> pair in hexGrid.tiles)
                //{
                   // Vector2Int key = pair.Key;
                   // HexTile tile = pair.Value;
                    //if (currentHexTile == tile || Vector2Int.Distance(tile.offsetCoordinate, currentHexTile.offsetCoordinate) > movedistance) continue;
                    //List<HexTile> Path = Pathfinder.FindPath(currentHexTile, tile);
                    //if (Path != null && Path.Count <= movedistance + 1)
                    //{
                    //    gridSystemVisual.ShowTile(key.x, key.y);
                    //}
                //}
            }
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


        public MoveAction GetMoveAction()
        {
            return moveAction;
        }
        public HexTile GetCurrentHexTile()
        {
            return currentHexTile;
        }
        public void SetCurrentHexTile(HexTile tile)
        {
            currentHexTile = tile;
            //gridSystemVisual.HideAllSingle();
        }
        public BaseAction[] GetBaseActionArray()
        {
            return baseActionArray;
        }
    }
}

