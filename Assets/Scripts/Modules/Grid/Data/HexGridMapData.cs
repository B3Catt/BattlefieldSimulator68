using System.Collections.Generic;

namespace BattlefieldSimulator
{
    public class HexGridMapData
    {
        HexGridSystem gridSystem;

        /// <summary>
        /// 
        /// </summary>
        private Dictionary<GridPosition, HexTile> tilesDictionary;

        public Dictionary<GridPosition , HexTile> TilesDictionary { get => tilesDictionary; }

        public HexGridMapData(HexGridSystem gridSystem)
        {
            this.gridSystem = gridSystem;
            tilesDictionary = new Dictionary<GridPosition, HexTile>();
        }

        public Dictionary<GridPosition, HexTile>.ValueCollection GetAllTiles() => tilesDictionary.Values;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gridPosition"></param>
        /// <param name="hexTile"></param>
        /// <returns></returns>
        public bool TryGetTileByGridPosition(GridPosition gridPosition, out HexTile hexTile)
        {
            if (!gridSystem.CheckPosition(gridPosition) || !tilesDictionary.ContainsKey(gridPosition))
            {
                hexTile = null;
                return false;
            }
            hexTile =  tilesDictionary[gridPosition];
            return true;
        }

        public bool TryAddTileByGridPosition(GridPosition gridPosition, HexTile hexTile)
        {
            if (!gridSystem.CheckPosition(gridPosition))
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
            foreach (var position in tilesDictionary.Keys)
            {
                if (gridSystem.CheckPosition(position))
                    temp.Add(position, tilesDictionary[position]);
            }
            tilesDictionary.Clear();
            tilesDictionary = temp;
        }
    }
}
