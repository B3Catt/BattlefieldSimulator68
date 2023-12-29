using UnityEngine;


namespace BattlefieldSimulator
{
    /// <summary>
    /// Unit; the object which Users can control;
    /// TODO: 完成对 Unit 类的设计
    /// </summary>
    public class Unit : MonoBehaviour
    {
        private HexTile currentHexTile;
        private MoveAction moveAction;
        private BaseAction[] baseActionArray;


        public int q;
        public int r;
        //public HexGrid hexGrid;
        public int movedistance = 4;
        public bool ifselected = false;
        //public GridSystemVisual gridSystemVisual;

        public bool isFlight = false;

        /// <summary>
        /// below just test
        /// </summary>
        private GridManager gridManager;
        public GridManager GridManager { get { return gridManager; } }

        public void Register(GridManager gridManager) => this.gridManager = gridManager;

        private void Awake()
        {
            moveAction = GetComponent<MoveAction>();
            baseActionArray = GetComponents<BaseAction>();
        }
        private void Start()
        {
            //设置初始位置
            //SetStartTile(q, r);
            moveAction.onMoveOneTile += gridManager.visualSystem.UpdateGridVisual;
        }
        private void Update()
        {
            if (ifselected)
            {
            }
        }

        public void SetStartTile(int q, int r)
        {
            GameObject gridObject = GameObject.Find("Terrain");
            if (gridObject != null)
            {
                Transform hexTileTransform = gridObject.transform.Find($"Tile_ {q}_{r}_{-q - r}");
                if (hexTileTransform != null)
                {
                    currentHexTile = hexTileTransform.GetComponent<HexTile>();
                    transform.position = currentHexTile.transform.position + new Vector3(0f, 1f, 0f);
                }
            }
        }

        public T GetAction<T>() where T : BaseAction
        {
            foreach (BaseAction baseAction in baseActionArray)
            {
                if (baseAction is T)
                {
                    return (T)baseAction;
                }
            }
            return null;
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
            currentHexTile = tile;
            gridManager.visualSystem.HideAllSingle();
        }
        public BaseAction[] GetBaseActionArray()
        {
            return baseActionArray;
        }
    }
}

