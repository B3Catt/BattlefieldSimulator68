using UnityEngine;

namespace BattlefieldSimulator
{
    public class HexGridMapGenerator
    {
        HexGridMapSetting mapSettings;
        HexTileGenerationSetting tileSettings;

        HexGridSystem gridSystem;
        public HexGridMapGenerator(HexGridMapSetting mapSettings, HexTileGenerationSetting tileSettings, HexGridSystem gridSystem)
        {
            this.mapSettings = mapSettings;
            this.tileSettings = tileSettings;
            this.gridSystem = gridSystem;
        }

        public bool Generate(HexGridMapData data)
        {
            if (data == null)
            {
                data = new HexGridMapData(mapSettings.width, mapSettings.height);
            }
            else if (data.width != mapSettings.width || data.height != mapSettings.height)
            {
                data.SetSize(mapSettings.width, mapSettings.height);
            }

            bool flag = true;
            for (int z = 0; z < mapSettings.height; z++)
            {
                for (int x = 0; x < mapSettings.width; x++)
                {
                    /// TODO: Set the Value of hex tile
                    /// 

                    GameObject tile = new GameObject($"Hex C{x},R{z}");

                    HexTile hexTile = tile.AddComponent<HexTile>();

                    hexTile.settings = tileSettings;

                    hexTile.radius = mapSettings.radius;
                    hexTile.height = 1;

                    hexTile.RollTileType();
                    hexTile.AddTile();

                    hexTile.gridPosition = new GridPosition(x, z);

                    tile.transform.position = gridSystem.GetPositionForHexFromCoordinate(hexTile.gridPosition);

                    tile.transform.SetParent(mapSettings.gridsTf, true);

                    if (!data.TryAddTileByGridPosition(hexTile.gridPosition, hexTile))
                    {
                        flag = false;
                    }
                }
            }

            return flag;
        }
    }
}
