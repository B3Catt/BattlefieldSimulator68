using UnityEngine;

namespace BattlefieldSimulator
{

    [ExecuteInEditMode]
    public class HexTile : Tile
    {
        public bool ifEmpty = true;
        public Unit unitOnIt = null;
        public void RegisterNeighbours()
        {
            neighbours = gridSystem.GetNeighbours(this);
        }

        private void Update()
        {
        }

        public void Destroy()
        {
            if (Application.isPlaying)
            {
                GameObject.Destroy(gameObject);
            }
            else
            {
                GameObject.DestroyImmediate(gameObject);
            }
        }
        public void OnHighlightTile()
        {
        }
        public void OnSelcetTile()
        {
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
