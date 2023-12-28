using UnityEngine;

namespace BattlefieldSimulator
{
    [CreateAssetMenu(menuName = "TileGen/GenerationSettings")]
    public class HexTileGenerationSetting : ScriptableObject                                                                                                                                                                                                                                                                                
    {
        public enum TileType
        {
            Standard,
            Sea,
            Cliff
        }

        /**
         * plane
         * marsh
         * mountain
         * sea
         * desert
         * forest
         * city
         */

        public Material Standard;
        public Material Water;
        public Material Cliff;

        public GameObject Mesh;

        public Material GetTileMaterial(TileType tileType)
        {
            switch (tileType)
            {
                case TileType.Standard: 
                    return Standard;
                case TileType.Sea:
                    return Water;
                case TileType.Cliff:
                    return Cliff;
            }
            return null;
        }
    }
}
