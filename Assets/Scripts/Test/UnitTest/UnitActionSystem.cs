using System;
using UnityEngine;

namespace BattlefieldSimulator
{
    public class UnitActionSystem : MonoBehaviour
    {
        public static UnitActionSystem Instance { get; private set; }

        public event EventHandler OnSelectedUnitChange;

        public event EventHandler OnSelectedActionChanged;
        [SerializeField] private UnitTest selectedUnit;
        [SerializeField] private LayerMask unitsLayerMask;

        private BaseAction selectedAction;
        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError("There's more than one UnitActionSystem! " + transform + "-" + Instance);
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        private void Start()
        {
            SetSelectedUnit(selectedUnit);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (selectedUnit != null && selectedUnit.GetMoveAction().IfMoving == true) return;
                if (TryHandleUnitSelection()) return;
                if (selectedUnit != null)
                {
                    selectedUnit.GetMoveAction().Move(MouseWorld.GetHexTile());
                }
            }
        }

        private bool TryHandleUnitSelection()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, unitsLayerMask))
            {
                if (raycastHit.transform.TryGetComponent<UnitTest>(out UnitTest unit))
                {
                    SetSelectedUnit(unit);
                    return true;
                }
            }
            return false;
        }


        private void HandleSelectedAction()
        {
            
            // if (InputManager.Instance.IsMouseButtonDownThisFrame())
            // {
            //     GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());

            //     if (!selectedAction.IsValidActionGridPosition(mouseGridPosition))
            //     {
            //         return;
            //     }

            //     if (!selectedUnit.TrySpendActionPointsToTakeAction(selectedAction))
            //     {
            //         return;
            //     }

            //     SetBusy();
            //     selectedAction.TakeAction(mouseGridPosition, ClearBusy);

            //     OnActionStarted?.Invoke(this, EventArgs.Empty);
            // }
        }
        private void SetSelectedUnit(UnitTest unit)
        {
            if (selectedUnit) selectedUnit.ifselected = false;

            selectedUnit = unit;

            selectedUnit.ifselected = true;

            SetSelectedAction(unit.GetAction<MoveAction>());

            OnSelectedUnitChange?.Invoke(this, EventArgs.Empty);
        }

        public void SetSelectedAction(BaseAction baseAction)
        {
            selectedAction = baseAction;

            OnSelectedActionChanged?.Invoke(this, EventArgs.Empty);
        }

        public UnitTest GetSelectedUnit()
        {
            return selectedUnit;
        }
    }
}
