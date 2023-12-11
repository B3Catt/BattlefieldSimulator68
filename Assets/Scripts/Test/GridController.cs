using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace BattlefieldSimulator
{
    public class GridController : BaseController
    {
        public Vector2Int oldGridSize { get; set; }

        public float radius { get; set; }
        public Transform parentTransform { get; set; }
        public bool isFlatTopped { get; set; }

        private Dictionary<Vector2Int, GameObject> grids;
        public GridController()
        {
            grids = new Dictionary<Vector2Int, GameObject>();
        }

        public void OnUpdate()
        {
        }

        public void LayoutGrid(Vector2Int gridSize, float radius, bool isFlatTopped, Transform parentTransform)
        {
            this.radius = radius;
            this.isFlatTopped = isFlatTopped;
            this.parentTransform = parentTransform;

            for (int y = 0; y < gridSize.y; y++)
            {
                for (int x = 0; x < gridSize.x; x++)
                {

                    GameObject tile;
                    Vector2Int point = new Vector2Int(x, y);
                    if (grids.ContainsKey(point))
                    {
                        tile = grids[point];
                    }
                    else
                    {
                        tile = new GameObject($"Hex {x}, {y}", typeof(MeshCollider), typeof(HexRenderer));
                        grids.Add(point, tile);
                    }

                    tile.transform.position = GetPositionForHexFromCoordinate(point);

                    HexRenderer hexRenderer = tile.GetComponent<HexRenderer>();
                    hexRenderer.isFlatTopped = isFlatTopped;
                    hexRenderer.outerSize = radius;
                    hexRenderer.innerSize = 0;
                    hexRenderer.pos = point;

                    int id = Random.Range(0, 3) + 1;
                    Terrain terrian = GameApp.ModelManager.GetDataById<Terrain>(id);
                    hexRenderer.height = terrian._height;


                    var tms = GameApp.ModelManager.GetData<TerrainModel>();
                    Material material = null;
                    foreach (var pair in tms)
                    {
                        if (pair.Value._terrain_id == id)
                        {
                            material = Resources.Load<Material>($"Materials/{pair.Value._model_id}");
                            break;
                        }
                    }

                    hexRenderer.terrian = terrian;

                    hexRenderer.SetMaterial(material);
                    hexRenderer.DrawMesh();

                    tile.transform.SetParent(parentTransform, true);
                }
            }

            if (oldGridSize.x > gridSize.x)
            {
                for (int y = 0; y < oldGridSize.y; ++y)
                {
                    for (int x = gridSize.x; x < oldGridSize.x; ++x)
                    {
                        var pos = new Vector2Int(x, y);
                        var tile = grids[pos];
                        HexRenderer hexRenderer = tile.GetComponent<HexRenderer>();
                        hexRenderer.DestroyGameObject();
                        grids.Remove(pos);
                    }
                }
            }

            oldGridSize.Set(gridSize.x, oldGridSize.y);

            if (oldGridSize.y > gridSize.y)
            {
                for (int y = gridSize.y; y < oldGridSize.y; ++y)
                {
                    for (int x = 0; x < oldGridSize.x; ++x)
                    {
                        var pos = new Vector2Int(x, y);
                        var tile = grids[pos];
                        HexRenderer hexRenderer = tile.GetComponent<HexRenderer>();
                        hexRenderer.DestroyGameObject();
                        grids.Remove(pos);
                    }
                }
            }

            oldGridSize = gridSize;
        }

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
            float size = radius;

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
    }
}


