using System;
using UnityEngine;

namespace BattlefieldSimulator
{
    public class UnitActionSystem : MonoBehaviour
    {
        public static UnitActionSystem Instance { get; private set; }

        public event EventHandler OnSelectedUnitChange;

        public event EventHandler OnSelectedActionChanged;
        [SerializeField] private Unit selectedUnit;
        [SerializeField] private LayerMask unitsLayerMask;
        private bool isBusy;

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
            if (isBusy) return;

            if (TryHandleUnitSelection()) return;

            HandleSelectedAction();
        }

        private bool TryHandleUnitSelection()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, unitsLayerMask))
                {
                    if (raycastHit.transform.TryGetComponent<Unit>(out Unit unit))
                    {
                        SetSelectedUnit(unit);
                        return true;
                    }
                }
            }
            return false;
        }


        private void HandleSelectedAction()
        {
            if (Input.GetMouseButtonDown(0))
            {
                SetBusy();
                selectedAction.TakeAction(MouseWorld.GetHexTile(), ClearBusy);
            }
        }
        private void SetSelectedUnit(Unit unit)
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

        public Unit GetSelectedUnit()
        {
            return selectedUnit;
        }

        public BaseAction GetSelectedAction()
        {
            return selectedAction;
        }
        private void SetBusy()
        {
            isBusy = true;

            //OnBusyChanged?.Invoke(this, isBusy);
        }

        private void ClearBusy()
        {
            isBusy = false;

            //OnBusyChanged?.Invoke(this, isBusy);
        }
    }
}
