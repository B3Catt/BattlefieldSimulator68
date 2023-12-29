using K4os.Compression.LZ4.Internal;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace BattlefieldSimulator
{
    public class GridManager
    {
        public HexGridSystem gridSystem { get; private set; }
        public HexGridVisualSystem visualSystem { get; private set; }

        public GridManager(GeneratorData data, Transform terrainObj, Transform visualObj, Transform singleVisualPrefab)
        {
            gridSystem = new HexGridSystem(data, terrainObj);
            visualSystem = new HexGridVisualSystem(gridSystem, singleVisualPrefab, visualObj);
            gridSystem.GenerateMap();
        }

        public void Intialize()
        {
            gridSystem.InitTiles();
            visualSystem.Initialize();
        }

    }
}


