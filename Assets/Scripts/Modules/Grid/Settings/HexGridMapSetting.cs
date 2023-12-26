using UnityEngine;

namespace BattlefieldSimulator
{

    [CreateAssetMenu(menuName = "HexTileGen/MapGenerationSettings")]
    public class HexGridMapSetting : ScriptableObject
    {
        public float radius;
        public bool isFlatTopped;

        public int width;
        public int height;

        public Transform gridsTf;
        public LayerMask layerMask;
    }
}
