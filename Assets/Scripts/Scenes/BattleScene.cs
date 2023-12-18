using UnityEngine;

namespace BattlefieldSimulator
{
    public class BattleScene : MonoBehaviour
    {
        [Header("Grid Setting")]
        public Vector2Int gridSize;

        [Header("Tile Settings")]
        public float outerSize = 15f;
        public float innerSize = 0f;
        public float height = 1f;
        public bool isFlatTopped;
        public Material material;

        private GridController gridController;

        private void OnEnable()
        {
            GameApp.ControllerManager.Register(ControllerType.Grid, new GridController());
            GameApp.ControllerManager.ApplyFunc(ControllerType.Grid, Defines.LayoutMapGrid, gridSize, outerSize, isFlatTopped, transform);
        }

        private void OnValidate()
        {
            if (Application.isPlaying)
            {
                GameApp.ControllerManager.ApplyFunc(ControllerType.Grid, Defines.LayoutMapGrid, gridSize, outerSize, isFlatTopped, transform);
            }
        }

        
    }
}
