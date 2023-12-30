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
        public HexGridSystem HexGridSystem { get { return gridSystem; } }
        private HexGridVisualSystem visualSystem;
        public HexGridVisualSystem HexGridVisualSystem { get { return visualSystem; } }

        public GridManager(GeneratorData data, Transform terrainObj, Transform visualObj, Transform singleVisualPrefab)
        {
            gridSystem = new HexGridSystem(data, terrainObj);
            visualSystem = new HexGridVisualSystem(gridSystem, singleVisualPrefab, visualObj);
        }

        public void Intialize()
        {
            gridSystem.GenerateMap();
            gridSystem.InitTiles();
            visualSystem.Initialize();
        }

    }
}


