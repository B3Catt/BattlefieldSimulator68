using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace BattlefieldSimulator
{
    public class TileManager : MonoBehaviour
    {
        public GameObject gameObject;
        public GameObject highlight;
        public GameObject selector;
        public PlayerCharacter player;
        public TileManager instance;
        public Dictionary<Vector3Int, HexTile> tiles;
        public List<HexTile> path;
        public bool ifselected;
        public HexTile Selected;
        public void Awake()
        {
            InitTiles();
        }

        private void InitTiles()
        {
            ifselected = false;
            instance = this;
            tiles = new Dictionary<Vector3Int, HexTile>();

            HexTile[] hexTiles = gameObject.GetComponentsInChildren<HexTile>();
            foreach (HexTile hexTile in hexTiles)
            {
                RegisterTile(hexTile);

            }
            foreach (HexTile hexTile in hexTiles)
            {
                List<HexTile> neighbours = GetNeighbours(hexTile);
                hexTile.neighbours = neighbours;
            }
            int i = Random.Range(0, 6);
            HexTile tile = hexTiles[i];
            player.playerPos = tile.cubeCoordinate;
            player.transform.position = tile.transform.position + new Vector3(0, 2f, 0);
            player.currenttile = tile;
        }
        private List<HexTile> GetNeighbours(HexTile tile)
        {
            List<HexTile> neighbours = new List<HexTile>();

            Vector3Int[] neighbourCoords = new Vector3Int[]
            {
                new Vector3Int(1, -1, 0),
                new Vector3Int(1, 0, -1),
                new Vector3Int(0, 1, -1),
                new Vector3Int(-1, 1, 0),
                new Vector3Int(-1, 0, 1),
                new Vector3Int(0, -1, 1)
            };

            foreach (Vector3Int neighbourCoord in neighbourCoords)
            {
                Vector3Int tileCorrd = tile.cubeCoordinate;

                if (tiles.TryGetValue(tileCorrd + neighbourCoord, out HexTile neighbour))
                {
                    neighbours.Add(neighbour);
                }
            }

            return neighbours;
        }
        public void RegisterTile(HexTile tile)
        {
            Vector3Int cubeCoordinate = tile.cubeCoordinate;

            if (tiles.ContainsKey(cubeCoordinate))
            {
                // 存在相同的 cubeCoordinate，可以更新或者忽略这个项
                tiles[cubeCoordinate] = tile;
                // 或者选择忽略这个 tile 的添加
            }
            else
            {
                tiles.Add(cubeCoordinate, tile);
            }
        }

        public void OnHighlightTile(HexTile tile)
        {
            highlight.transform.position = tile.transform.position;
        }
        public void OnSelectTile(HexTile tile)
        {
            Debug.Log($"select  {tile.offsetCoordinate}");
            if (!ifselected)
            {
                selector.transform.position = tile.transform.position;
                Selected = tile;
                ifselected = true;
            }
            else
            {
                path = Pathfinder.FindPath(Selected, tile);
                if(path == null)
                {
                    Debug.Log("unable");
                    selector.transform.position=new Vector3(-200f,0f,5f);
                }
                else
                {
                    selector.transform.position=new Vector3(-200f,0f,5f);
                    OnDrawPath();
                }
                ifselected = false;
            }
        }
        public void OnDrawPath()
        {
            if (path != null)
            {
                Debug.Log("drawing Path");
                foreach (HexTile tile in path)
                {
                    Debug.Log($"{tile.offsetCoordinate}");
                }
            }
        }
    }
}