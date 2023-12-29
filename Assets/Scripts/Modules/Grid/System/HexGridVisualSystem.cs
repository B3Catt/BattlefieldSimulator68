using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace BattlefieldSimulator
{
    public class HexGridVisualSystem
    {
        [SerializeField] private Transform singlePrefab;
        [SerializeField] private Transform gridVisualParent;
        private Dictionary<GridPosition, GridVisualSingle> gridVisualDictionary;
        private HexGridSystem gridSystem;

        public enum GridVisualType
        {
            White,
            Blue,
            Red,
            RedSoft,
            Yellow,
        }
        public HexGridVisualSystem(HexGridSystem gridSystem, Transform singlePrefab, Transform gridVisualParent)
        {
            this.singlePrefab = singlePrefab;
            this.gridVisualParent = gridVisualParent;
            this.gridSystem = gridSystem;
        }

        public void Initialize()
        {
            gridVisualDictionary = new Dictionary<GridPosition, GridVisualSingle>();
            foreach (KeyValuePair<GridPosition, HexTile> pair in gridSystem.Data.TilesDictionary)
            {
                GridPosition key = pair.Key;
                HexTile value = pair.Value;
                Transform singleTransform =
                    UnityEngine.Object.Instantiate(singlePrefab, gridSystem.GetLocalPositionForHexFromCoordinate(key) + new Vector3(0, 0.5f, 0),
                    Quaternion.Euler(gridSystem.GeneratorData.topografiaSettings.rotation), gridVisualParent);
                
                singleTransform.localScale = Vector3.one * gridSystem.GeneratorData.topografiaSettings.baseScale;
                if (!singleTransform.TryGetComponent(out GridVisualSingle single))
                {
                    single = singleTransform.gameObject.AddComponent<GridVisualSingle>();
                }
                gridVisualDictionary.Add(key, single);
            }

            UnitActionSystem.Instance.OnSelectedUnitChange += UnitActionSystem_OnSelectedUnitChanged;
            UpdateGridVisual();
        }

        public void HideAllSingle()
        {
            foreach (var single in  gridVisualDictionary.Values)
            {
                single.hide();
            }
        }

        public void ShowTile(GridPosition gridPosition)
        {
            if (gridVisualDictionary.TryGetValue(gridPosition, out GridVisualSingle single))
            {
                single.show();
            }
        }
        private void UnitActionSystem_OnSelectedUnitChanged(object sender, EventArgs e)
        {
            HideAllSingle();
        }

        public void UpdateGridVisual()
        {
            HideAllSingle();

            Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();
            BaseAction selectedAction = UnitActionSystem.Instance.GetSelectedAction();
            GridVisualType gridVisualType = GridVisualType.White;
            ShowGridPositionList(selectedAction.GetValidActionGridPositionList(), gridVisualType);
        }
        public void ShowGridPositionList(List<HexTile> tileList, GridVisualType gridVisualType)
        {
            foreach (HexTile tile in tileList)
            {
                if (gridVisualDictionary.TryGetValue(tile.GridPosition, out GridVisualSingle single))
                {
                    single.show();
                }
            }
        }
    }
}
