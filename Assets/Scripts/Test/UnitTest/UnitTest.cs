using UnityEngine;

namespace BattlefieldSimulator
{
    public class UnitTest : MonoBehaviour
    {
        private HexTile currentHexTile;
        private MoveAction moveAction;
        private void Awake()
        {
            moveAction = GetComponent<MoveAction>();
        }
        private void Start()
        {
            GameObject gridObject = GameObject.Find("grid");

            if (gridObject != null)
            {
                // 在 Grid 对象下找到名为 "Hex C0,R0" 的子对象
                Transform hexTileTransform = gridObject.transform.Find("Hex C0,R0");

                if (hexTileTransform != null)
                {
                    // 获取 HexTile 组件
                    currentHexTile = hexTileTransform.GetComponent<HexTile>();
                }
                else
                {
                    // 没有找到名为 "Hex C0,R0" 的子对象
                }
            }
            else
            {
                // 没有找到名为 "Grid" 的对象
            }
        }
        private void Update()
        {
        }

        public MoveAction GetMoveAction()
        {
            return moveAction;
        }
        public HexTile GetCurrentHexTile()
        {
            return currentHexTile;
        }

        public void SetCurrentHexTile(HexTile tile)
        {
            currentHexTile=tile;
        }
    }
}

