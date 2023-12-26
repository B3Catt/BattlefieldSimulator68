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
            generator.Generate(data);
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
            float yPosition;
            bool shouldOffset;
            float horizontalDistance;
            float verticalDistance;
            float offset;
            float size = mapSettings.radius;

            if (!mapSettings.isFlatTopped)
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
