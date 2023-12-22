using System.Collections.Generic;
using UnityEngine;

namespace BattlefieldSimulator
{
    public class TileManager : MonoBehaviour
    {
        public GameObject gameObject;
        public GameObject highlight;
        public GameObject selector;

        public TileManager instance;
        public Dictionary<Vector3Int, HexTile> tiles;
        public void Awake()
        {
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
            tiles.Add(tile.cubeCoordinate, tile);
        }

        public void OnHighlightTile(HexTile tile)
        {
            highlight.transform.position = tile.transform.position;
        }
        public void OnSelectTile(HexTile tile)
        {
            selector.transform.position = tile.transform.position;
        }
    }
}