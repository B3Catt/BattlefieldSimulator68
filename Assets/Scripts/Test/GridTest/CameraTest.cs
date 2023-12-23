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
                if (objectHit.TryGetComponent<HexTile>(out target))
                {
                    target.OnHighlightTile();
                }
            }
            // 检测鼠标左键点击
            if (Input.GetMouseButtonDown(0))
            {
                // 检测是否点击到了物体
                if (Physics.Raycast(ray, out hit))
                {
                    // 获取点击到的物体的 HexTile 组件
                    HexTile hexTile = hit.collider.GetComponent<HexTile>();

                    // 检查是否成功获取到 HexTile 组件
                    if (hexTile != null)
                    {
                        // 调用 OnSelectTile 方法
                        hexTile.OnSelcetTile();
                    }
                }
            }
        }
    }
}
