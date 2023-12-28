using K4os.Compression.LZ4.Internal;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace BattlefieldSimulator
{
    public class GridManager
    {
        private GeneratorData data;
        private Transform terrainObj;
        public HexGridSystem gridSystem { get; private set; }

        public GridManager(GeneratorData data, Transform terrainObj)
        {
            this.data = data;
            this.terrainObj = terrainObj;
            gridSystem = new HexGridSystem(data, terrainObj);
        }

    }
}


