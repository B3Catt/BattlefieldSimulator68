using UnityEngine;

namespace BattlefieldSimulator
{
    public class GridVisualSingle : MonoBehaviour
    {
        [SerializeField] private Renderer renderer;
        private void Awake()
        {
            renderer = GetComponentInChildren<Renderer>();
        }

        public void show()
        {
            renderer.enabled = true;
        }

        public void hide()
        {
            renderer.enabled = false;
        }
    }
}
