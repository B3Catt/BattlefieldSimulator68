//using System.Collections.Generic;
//using System.IO;
//using UnityEngine;

//namespace BattlefieldSimulator
//{
//    public class TileManager : MonoBehaviour
//    {
//        public GameObject gameObject;
//        public GameObject highlight;
//        public GameObject selector;
//        // public PlayerCharacter player;
//        // public PlayerCharacter player2;

//        public TileManager instance;
//        public Dictionary<Vector2Int, HexTile> tiles;
//        public List<HexTile> path;
//        // public bool ifselected;
//        // public HexTile Selected;
//        public void Awake()
//        {
//            InitTiles();
//        }

//        private void InitTiles()
//        {
//            //ifselected = false;
//            instance = this;
//            tiles = new Dictionary<Vector2Int, HexTile>();

//            HexTile[] hexTiles = gameObject.GetComponentsInChildren<HexTile>();
//            foreach (HexTile hexTile in hexTiles)
//            {
//                RegisterTile(hexTile);

//            }
//            // int i = Random.Range(0, 8);
//            // HexTile tile = hexTiles[i];
//            // player.transform.position = tile.transform.position + new Vector3(0, 2f, 0);
//            // player.currenttile = tile;
//            // i = Random.Range(0, 8);
//            // tile = hexTiles[i];
//            // player2.transform.position = tile.transform.position + new Vector3(0, 2f, 0);
//            // player2.currenttile = tile;
//        }
//        public void RegisterTile(HexTile tile)
//        {
//            Vector2Int offsetCoordinate = tile.offsetCoordinate;

//            if (tiles.ContainsKey(offsetCoordinate))
//            {
//                // 存在相同的 cubeCoordinate，可以更新或者忽略这个项
//                tiles[offsetCoordinate] = tile;
//                // 或者选择忽略这个 tile 的添加
//            }
//            else
//            {
//                tiles.Add(offsetCoordinate, tile);
//            }
//        }

//        public void OnHighlightTile(HexTile tile)
//        {
//            highlight.transform.position = tile.transform.position + new Vector3(0f, 1.5f, 0f);
//        }
//        //un
//        // public void OnSelectTile(HexTile tile)
//        // {
//        //     Debug.Log($"select  {tile.offsetCoordinate}");
//        //     if (!ifselected)
//        //     {
//        //         selector.transform.position = tile.transform.position + new Vector3(0f, 1.5f, 0f);
//        //         Selected = tile;
//        //         ifselected = true;
//        //     }
//        //     else
//        //     {
//        //         path = Pathfinder.FindPath(Selected, tile);
//        //         if (path == null)
//        //         {
//        //             Debug.Log("unable");
//        //             selector.transform.position = new Vector3(-200f, 0f, 5f);
//        //         }
//        //         else
//        //         {
//        //             selector.transform.position = new Vector3(-200f, 0f, 5f);
//        //             OnDrawPath();
//        //         }
//        //         ifselected = false;
//        //     }
//        // }
//        // //un
//        // public void OnDrawPath()
//        // {
//        //     if (path != null)
//        //     {
//        //         Debug.Log("drawing Path");
//        //         foreach (HexTile tile in path)
//        //         {
//        //             Debug.Log($"{tile.offsetCoordinate}");
//        //         }
//        //     }
//        // }
//    }
//}