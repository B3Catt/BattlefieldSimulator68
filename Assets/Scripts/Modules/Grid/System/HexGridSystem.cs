using System.Collections.Generic;
using UnityEngine;

namespace BattlefieldSimulator
{
    public class HexGridSystem
    {
        private HexGridMapSetting mapSettings;

        private HexGridMapGenerator generator;
        private HexGridMapData data;
        private bool dirtyFlag;
        public HexGridSystem(HexGridMapSetting mapSettings, HexTileGenerationSetting tileSettings)
        {
            this.mapSettings = mapSettings;
            generator = new HexGridMapGenerator(mapSettings, tileSettings, this);
            dirtyFlag = false;
        }

        public void Generate() => generator.Generate(data);

        public void InitTiles()
        {
            if (data == null)
            {
                return;
            }

            foreach (var hexTile in data.GetAllTiles())
            {
                hexTile.neighbours = GetNeighbours(hexTile);
            }
        }

        public void LoadMap(HexGridMapData data)
        {
            this.data = data;
            mapSettings.width = data.width;
            mapSettings.height = data.height;
        }

        private List<HexTile> GetNeighbours(HexTile tile)
        {
            List<HexTile> neighbours = new List<HexTile>();
            Vector3Int[] neighbourCoordinary = new Vector3Int[]
                {
                    new Vector3Int(1, -1, 0),
                    new Vector3Int(1, 0, -1),
                    new Vector3Int(0, 1, -1),
                    new Vector3Int(-1, 1, 0),
                    new Vector3Int(-1, 0, 1),
                    new Vector3Int(0, -1, 1)
                };


            foreach (Vector3Int coord in neighbourCoordinary)
            {
                GridPosition gridPosition = tile.gridPosition;

                if (data.TryGetTileByGridPosition(gridPosition + coord, out HexTile neighbour))
                {
                    neighbours.Add(neighbour);
                }
            }

            return neighbours;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gridPosition"></param>
        /// <returns></returns>
        public Vector3 GetPositionForHexFromCoordinate(GridPosition gridPosition)
        {
            int column = gridPosition.x;
            int row = gridPosition.z;
            float width;
            float height;
            float xPosition;
            float zPosition;
            float yPosition;
            bool shouldOffset;
            float horizontalDistance;
            float verticalDistance;
            float offset;
            float size = mapSettings.radius;

            if ((data?.TryGetTileByGridPosition(gridPosition, out HexTile hexTile))??false)
            {
                yPosition = hexTile.height;
            }
            else
            {
                yPosition = 0;
            }

            if (!mapSettings.isFlatTopped)
            {
                shouldOffset = row % 2 == 0;
                width = Mathf.Sqrt(3) * size;
                height = 2f * size;

                horizontalDistance = width;
                verticalDistance = height * (3f / 4f);

                offset = shouldOffset ? width / 2 : 0;

                xPosition = (column * horizontalDistance) + offset;
                zPosition = (row * verticalDistance);
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
                zPosition = (row * verticalDistance) - offset;
            }

            return new Vector3(xPosition, yPosition, -zPosition);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hexTile"></param>
        /// <returns></returns>
        public bool TryGetHexTileByMousePosition(out HexTile hexTile)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, float.MaxValue, mapSettings.layerMask))
            {
                Transform objectHit = hit.transform;

                if (objectHit.TryGetComponent<HexTile>(out HexTile target))
                {
                    hexTile = target;
                    return true;
                }
            }
            hexTile = null;
            return false;
        }
    }
}
