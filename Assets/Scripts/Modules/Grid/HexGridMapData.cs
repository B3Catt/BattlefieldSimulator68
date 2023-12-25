using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

namespace BattlefieldSimulator
{
    public class HexGridMapData
    {
        /// <summary>
        /// 
        /// </summary>
        public int width { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public int height { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        private Dictionary<GridPosition, HexTile> tilesDictionary;

        public HexGridMapData(int width, int height)
        {
            this.width = width;
            this.height = height;
            tilesDictionary = new Dictionary<GridPosition, HexTile>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gridPosition"></param>
        /// <param name="hexTile"></param>
        /// <returns></returns>
        public bool TryGetTileByGridPosition(GridPosition gridPosition, out HexTile hexTile)
        {
            if (gridPosition.x < 0 || gridPosition.x >= width || gridPosition.z < 0 || gridPosition.z >= height || !tilesDictionary.ContainsKey(gridPosition))
            {
                hexTile = null;
                return false;
            }
            hexTile =  tilesDictionary[gridPosition];
            return true;
        }

        public bool TryAddTileByGridPosition(GridPosition gridPosition, HexTile hexTile)
        {
            if (gridPosition.x < 0 || gridPosition.x >= width || gridPosition.z < 0 || gridPosition.z >= height)
            {
                hexTile = null;
                return false;
            }
            if (tilesDictionary.ContainsKey(gridPosition))
            {
                tilesDictionary[gridPosition] = hexTile;
            }
            else
            {
                tilesDictionary.Add(gridPosition, hexTile);
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void SetSize(int width, int height)
        {
            this.width = width;
            this.height = height;

            CacheFlush();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Clear()
        {
            foreach (var tile in tilesDictionary.Values)
            {
                tile.Destroy();
            }
            tilesDictionary.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        public void CacheFlush()
        {
            Dictionary<GridPosition, HexTile> temp = new Dictionary<GridPosition, HexTile>();
            for (int z = 0; z < height; z++)
            {
                for (int x = 0; x < width; x++)
                {
                    GridPosition gridPosition = new GridPosition(x, z);
                    if (tilesDictionary.ContainsKey(gridPosition))
                    {
                        temp.Add(gridPosition, tilesDictionary[gridPosition]);
                    }
                }
            }
            tilesDictionary.Clear();
            tilesDictionary = temp;
        }
    }
}
