using K4os.Compression.LZ4.Internal;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace BattlefieldSimulator
{
    public static class Direction
    {
        public static List<Vector2Int> direcitonOffsetOddPointTop = new List<Vector2Int>
        {
            new Vector2Int(-1, 1),
            new Vector2Int(0, 1),
            new Vector2Int(-1, 0),
            new Vector2Int(1, 0),
            new Vector2Int(-1, 1),
            new Vector2Int(0, -1)
        };

        public static List<Vector2Int> direcitonOffsetEvenPointTop = new List<Vector2Int>
        {
            new Vector2Int(0, 1),
            new Vector2Int(1, 1),
            new Vector2Int(-1, 0),
            new Vector2Int(1, 0),
            new Vector2Int(0, 1),
            new Vector2Int(1, -1)
        };

        public static List<Vector2Int> direcitonOffsetOddFlatTop = new List<Vector2Int>
        {
            new Vector2Int(1, 0),
            new Vector2Int(1, 1),
            new Vector2Int(0, -1),
            new Vector2Int(0, 1),
            new Vector2Int(-1, 0),
            new Vector2Int(-1, 1)
        };

        public static List<Vector2Int> direcitonOffsetEvenFlatTop = new List<Vector2Int>
        {
            new Vector2Int(1, 0),
            new Vector2Int(1, -1),
            new Vector2Int(0, -1),
            new Vector2Int(0, 1),
            new Vector2Int(-1, 0),
            new Vector2Int(-1, -1)
        };
    }
    public class GridManager : BaseController
    {
        public Vector2Int oldGridSize { get; set; }

        public float radius { get; set; }
        public Transform parentTransform { get; set; }
        public bool isFlatTopped { get; set; }

        private Dictionary<Vector2Int, GameObject> grids;

        private bool choseFirst = false;

        private Vector2Int FistGridPos;

        public HexTileGenerationSetting settings;
        public GridManager()
        {
            grids = new Dictionary<Vector2Int, GameObject>();

            InitModuleEvent();
            InitGlobalEvent();
        }

        public override void InitModuleEvent()
        {
            base.InitModuleEvent();

            RegisterFunc(Defines.LayoutMapGrid, LayoutGrid);
            RegisterFunc(Defines.ClickOnGrid, ClickOnGrid);
        }

        public override void InitGlobalEvent()
        {

        }

        private void LayoutGrid(object[] args)
        {
            Vector2Int gridSize = (args[0] as Vector2Int?)??(new Vector2Int(1, 1));
            radius = (args[1] as float?) ?? 1;
            isFlatTopped = (args[2] as bool?) ?? true;
            parentTransform = args[3] as Transform;

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
                        tile = new GameObject($"Hex {x}, {y}", typeof(HexRenderer));
                        grids.Add(point, tile);
                    }

                    tile.transform.position = GetPositionForHexFromCoordinate(point);

                    HexRenderer hexRenderer = tile.GetComponent<HexRenderer>();
                    hexRenderer.isFlatTopped = isFlatTopped;
                    hexRenderer.outerSize = radius;
                    hexRenderer.innerSize = 0;
                    hexRenderer.pos = point;

                    /**
                     * here is a test 
                     */
                    test_of_terrain_system(hexRenderer);

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

        private void ClickOnGrid(object[] args)
        {
            Vector2Int pos = (args[0] as Vector2Int?) ?? (new Vector2Int(0, 0));

            if (!choseFirst)
            {
                choseFirst = true;
                FistGridPos = pos;
                return;
            }

            choseFirst = false;
            List<Vector2Int> path = new List<Vector2Int>();
            bool[,] flags = new bool[oldGridSize.x, oldGridSize.y];
            for (int i = 0; i < oldGridSize.x; i++)
            {
                for (int j = 0; j < oldGridSize.y; j++)
                {
                    flags[i, j] = false;
                }
            }


            bool success = BFS(FistGridPos, pos, flags, path);

            if (success)
            {
                int i = 1;
                foreach (var item in path)
                {
                    Debug.Log($"Path{i++} {item.x}, {item.y}");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="des"></param>
        /// <param name="flags"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        private bool DFS(Vector2Int pos, Vector2Int des, bool[,] flags, List<Vector2Int> path)
        {
            // add it into path
            if (!flags[pos.x, pos.y])
            {
                flags[pos.x, pos.y] = true;
                path.Add(pos);
            }

            if (pos == des)
            {
                return true;
            }

            List<Vector2Int> neighbors = GetNeighbors(pos);
            foreach (var neighbor in neighbors)
            {
                Vector2Int temp = neighbor + pos;
                // test if it's passable
                if (temp.x < 0 || temp.y < 0 
                    || temp.x >= oldGridSize.x
                    || temp.y >= oldGridSize.y
                    || flags[temp.x, temp.y]
                    )
                {
                    continue;
                }

                if (DFS(pos + neighbor, des, flags, path))
                {
                    return true;
                }
            }
            path.Remove(pos);
            flags[pos.x, pos.y] = false;
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="des"></param>
        /// <param name="flags"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        private bool BFS(Vector2Int pos, Vector2Int des, bool[,] flags, List<Vector2Int> path)
        {
            Queue<Vector2Int> queue = new Queue<Vector2Int>();
            Dictionary<Vector2Int, Vector2Int> father = new Dictionary<Vector2Int, Vector2Int>();

            queue.Enqueue(pos);
            flags[pos.x, pos.y] = true;
            bool flag = false;

            while(queue.Count > 0)
            {
                Vector2Int temp = queue.Dequeue();
                List<Vector2Int> neighbors = GetNeighbors(temp);

                if (temp ==  des)
                {
                    flag = true;
                    break;
                }

                foreach (var neighbor in neighbors)
                {
                    Vector2Int temp2 = neighbor + temp;
                    if (temp2.x < 0 || temp2.y < 0
                    || temp2.x >= oldGridSize.x
                    || temp2.y >= oldGridSize.y
                    || flags[temp2.x, temp2.y]
                    )
                    {
                        continue;
                    }
                    queue.Enqueue(temp2);
                    flags[temp2.x, temp2.y] = true;
                    father.Add(temp2, temp);
                }
            }

            if (flag)
            {
                Vector2Int recall = des;
                path.Add(recall);
                while (recall != pos)
                {
                    recall = father[recall];
                    path.Add(recall);
                }
                path.Reverse();
            }

            return flag;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        private List<Vector2Int> GetNeighbors(Vector2Int pos)
        {
            if (isFlatTopped)
            {
                if (pos.x % 2 == 0)
                {
                    return Direction.direcitonOffsetEvenFlatTop;
                }
                return Direction.direcitonOffsetOddFlatTop;
            }
            if (pos.y % 2 == 0)
            {
                return Direction.direcitonOffsetEvenPointTop;
            }
            return Direction.direcitonOffsetOddPointTop;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hexRenderer"></param>
        private void test_of_terrain_system(HexRenderer hexRenderer)
        {
            int id = Random.Range(1, 4);
            Terrain terrian = InstanceManager.ModelManager.GetDataById<Terrain>(id);
            hexRenderer.height = terrian._height;


            var tms = InstanceManager.ModelManager.GetData<TerrainModel>();
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
        }

    }
}


