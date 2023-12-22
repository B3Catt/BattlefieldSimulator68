using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace BattlefieldSimulator
{
    [ExecuteInEditMode]
    public class HexGrid : MonoBehaviour
    {
        [Header("Grid Setting")]
        public Vector2Int gridSize;

        [Header("Tile Settings")]
        public float outerSize = 1f;
        public float innerSize = 0f;
        public float height = 1f;
        public bool isFlatTopped;
        public Material material;
        public HexTileGenerationSetting settings;
        public TileManager tileManager;

        private Dictionary<Vector3Int, HexTile> tilesCubeCoordDictionary = new Dictionary<Vector3Int, HexTile>();
        private Dictionary<Vector2Int, HexTile> tilesOffsetCoordDictionary = new Dictionary<Vector2Int, HexTile>();

        private void OnEnable()
        {
            LayoutGrid();
        }

        private void OnValidate()
        {
            if (Application.isPlaying)
            {
                LayoutGrid();
            }
        }

        [EditorButton]
        private void LayoutGrid()
        {
            Clear();
            for (int y = 0; y < gridSize.y; y++)
            {
                for (int x = 0; x < gridSize.x; x++)
                {

                    GameObject tile = new GameObject($"Hex C{x},R{y}");

                    HexTile hexTile = tile.AddComponent<HexTile>();

                    hexTile.settings = settings;

                    hexTile.radius = outerSize;
                    hexTile.height = height;

                    hexTile.RollTileType();
                    hexTile.AddTile();

                    hexTile.offsetCoordinate = new Vector2Int(x, y);

                    hexTile.cubeCoordinate = UtilsClass.offsetToCubePointTop(hexTile.offsetCoordinate);


                    tile.transform.position = GetPositionForHexFromCoordinate(hexTile.offsetCoordinate);

                    tile.transform.SetParent(transform, true);

                    if (tilesCubeCoordDictionary.ContainsKey(hexTile.cubeCoordinate))
                    {
                        tilesCubeCoordDictionary[hexTile.cubeCoordinate] = hexTile;
                    }
                    else
                    {
                        tilesCubeCoordDictionary.Add(hexTile.cubeCoordinate, hexTile);
                    }

                    if (tilesOffsetCoordDictionary.ContainsKey(hexTile.offsetCoordinate))
                    {
                        tilesOffsetCoordDictionary[hexTile.offsetCoordinate] = hexTile;
                    }
                    else
                    {
                        tilesOffsetCoordDictionary.Add(hexTile.offsetCoordinate, hexTile);
                    }

                }
            }
            foreach (var pair in tilesCubeCoordDictionary)
            {
                pair.Value.neighbours = GetNeighbours(pair.Value);
                pair.Value.InitializeTile(tileManager);
                //pair.Value.tileManager.Awake();
            }
            //i=false;
            // HexTile[] hexTiles = GameObject.FindObjectsOfType<HexTile>();
            // foreach (HexTile hexTile in hexTiles)
            // {
            //     hexTile.Init();
            // }
        }

        /// <summary>
        /// even-r horizontal & odd-q vertical layout
        /// </summary>
        /// <param name="vector2Int"></param>
        /// <returns></returns>
        private Vector3 GetPositionForHexFromCoordinate(Vector2Int vector2Int)
        {
            int column = vector2Int.x;
            int row = vector2Int.y;
            float width;
            float height;
            float xPosition;
            float yPosition;
            bool shouldOffset;
            float horizontalDistance;
            float verticalDistance;
            float offset;
            float size = outerSize;

            if (!isFlatTopped)
            {
                shouldOffset = row % 2 == 0;
                width = Mathf.Sqrt(3) * size;
                height = 2f * size;

                horizontalDistance = width;
                verticalDistance = height * (3f / 4f);

                offset = shouldOffset ? width / 2 : 0;

                xPosition = (column * horizontalDistance) + offset;
                yPosition = (row * verticalDistance);
            }
            else
            {
                shouldOffset = column % 2 == 0;
                width = 2f * size;
                height = Mathf.Sqrt(3) * size;

                horizontalDistance = width * (3f / 4f);
                verticalDistance = height;

                offset = shouldOffset ? height / 2 : 0;

                xPosition = (column * horizontalDistance);
                yPosition = (row * verticalDistance) - offset;
            }

            return new Vector3(xPosition, 0, -yPosition);
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

                if (tilesCubeCoordDictionary.TryGetValue(tileCorrd + neighbourCoord, out HexTile neighbour))
                {
                    neighbours.Add(neighbour);
                }
            }

            return neighbours;
        }

        private void Clear()
        {
            for (int i = transform.childCount - 1; i >= 0; --i)
            {
                (transform.GetChild(i).gameObject.GetComponent<HexTile>()).Destroy();
                tilesCubeCoordDictionary.Clear();
                tilesOffsetCoordDictionary.Clear();
            }
        }
    }
}
