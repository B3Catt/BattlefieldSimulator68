using Polygen.HexagonGenerator;
using System.Collections.Generic;
using UnityEngine;

namespace BattlefieldSimulator
{
    public class Tile : MonoBehaviour
    {

        public float height {  get; protected set; }
        public List<HexTile> neighbours { get; protected set; }

        protected BiomeDataSettings biomeData;
        public BiomeDataSettings BiomeData
        {
            get => biomeData;
        }

        protected bool isSeaTile;

        public bool IsSeaTile { get => isSeaTile; }

        protected Color color;
        public Color Color
        {
            get => color;
        }

        protected HexGridSystem gridSystem;

        protected GridPosition gridPosition;
        public GridPosition GridPosition
        {
            get => gridPosition;
        }

        protected bool disable;
        public bool Disable { get => disable; }

        public void Initialize(BiomeDataSettings biomeData, GridPosition gridPosition, float height, Color color, bool isSeaTile, HexGridSystem gridSystem, bool disable)
        {
            this.biomeData = biomeData;
            this.gridPosition = gridPosition;

            this.height = height;
            this.color = color;

            this.isSeaTile = isSeaTile;
            this.gridSystem = gridSystem;

            this.disable = disable;

            if (DebugManager.debugNotActivated)
                return;

            TMPro.TextMeshPro textMeshPro = Instantiate(DebugManager.debugText, transform).GetComponent<TMPro.TextMeshPro>();

            switch (DebugManager.debugMode)
            {
                case DebugManager.DebugMode.Default:
                    textMeshPro.text = biomeData.biomeName;
                    break;
                case DebugManager.DebugMode.Cubic:
                    textMeshPro.text = gridPosition.ToString();
                    textMeshPro.rectTransform.sizeDelta = new Vector2(1.6f, textMeshPro.rectTransform.sizeDelta.y);
                    break;
                case DebugManager.DebugMode.Grid:
                    textMeshPro.text = gridPosition.ToString();
                    break;
                case DebugManager.DebugMode.Height:
                    textMeshPro.text = height.ToString("#.##");
                    break;
                default:
                    break;
            }
            //textMeshPro.transform.localScale = new Vector3(1, 1 / transform.localScale.y, 1);
        }

        public void SetHeight(float height)
        {
            this.height = height;
        }
        public void SetNeightbours(List<HexTile> neighbours)
        {
            this.neighbours = neighbours;
        }

        public override string ToString()
        {
            return $"Hex: {transform.gameObject.name}\n" +
                $"Position: {gridPosition}\n" +
                $"Height: {height}\n" +
                $"IsSeaTile: {isSeaTile}\n" +
                $"Color: {Color}";
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            //TODO: Respond height changes and detect correct biome based on height value.
        }
#endif
    }
}
