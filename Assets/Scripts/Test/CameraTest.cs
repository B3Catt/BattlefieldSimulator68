using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

namespace BattlefieldSimulator
{
    public class CameraTest : MonoBehaviour
    {
        public Camera _camera;

        public void Start()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            RaycastHit hit;
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                Transform objectHit = hit.transform;

                HexTile target;
                if(objectHit.TryGetComponent<HexTile>(out target))
                {
                    target.OnHighlightTile();
                }
            }
        }
    }
}
