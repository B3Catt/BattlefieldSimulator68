using K4os.Compression.LZ4.Internal;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace BattlefieldSimulator
{
    public class GridManager
    {
        private HexGridSystem gridSystem;
        private HexTileGenerationSetting gridSettings;
        private HexGridMapSetting mapSettings;

        public GridManager(HexTileGenerationSetting gridSettings, HexGridMapSetting mapSettings)
        {
            this.gridSettings = gridSettings;
            this.mapSettings = mapSettings;
            gridSystem = new HexGridSystem(mapSettings, gridSettings);
        }



    }
}


