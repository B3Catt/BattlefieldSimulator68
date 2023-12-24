using System;
using UnityEngine;

namespace BattlefieldSimulator
{
    public class UnitActionSystem : MonoBehaviour
    {
        public static UnitActionSystem Instance { get; private set; }

        public event EventHandler OnSelectedUnitChange;


        [SerializeField] private UnitTest selectedUnit;
        [SerializeField] private LayerMask unitsLayerMask;

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

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (TryHandleUnitSelection()) return;
                if (selectedUnit != null) selectedUnit.Move(MouseWorld.GetPosition());
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
        private void SetSelectedUnit(UnitTest unit)
        {
            selectedUnit = unit;
            OnSelectedUnitChange?.Invoke(this, EventArgs.Empty);
        }

        public UnitTest GetSelectedUnit()
        {
            return selectedUnit;
        }
    }
}