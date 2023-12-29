using System.Collections;
using System;
using System.Collections.Generic;
using BattlefieldSimulator;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace BattlefieldSimulator
{

    public class UnitActionSystemUI : MonoBehaviour
    {
        public UnitActionSystem unitActionSystem;
        [SerializeField] private Transform actionButtonPrefab;
        [SerializeField] private Transform actionButtonContainerTransform;
        [SerializeField] private TextMeshProUGUI actionPointsText;

        private List<ActionButtonUI> actionButtonUIList;

        private void Awake()
        {
            actionButtonUIList = new List<ActionButtonUI>();
        }
        private void Start()
        {
            UnitActionSystem.Instance.OnSelectedUnitChange += UnitActionSystem_OnSelectedUnitChanged;
            UnitActionSystem.Instance.OnSelectedActionChanged += UnitActionSystem_OnSelectedActionChanged;
            UnitActionSystem.Instance.OnActionStarted += UnitActionSystem_OnActionStarted;
            TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;//when next turn updata actionpoints
            UnitTest.OnAnyActionPointsChanged += Unit_OnAnyActionPointsChanged;//when actionpoints changed updata actionpoints

            CreateUnitActionButtons();
            UpdateSelectedVisual();
            UpdateActionPoints();
        }
        private void CreateUnitActionButtons()
        {
            foreach (Transform buttonTransform in actionButtonContainerTransform)
            {
                Destroy(buttonTransform.gameObject);
            }
<<<<<<< HEAD:Assets/Scripts/Modules/Unit/Behaviour/UnitActionSystemUI.cs
            Unit selectunit = unitActionSystem.GetSelectedUnit();
=======
            actionButtonUIList.Clear();
            UnitTest selectunit = unitActionSystem.GetSelectedUnit();
>>>>>>> 35081e962ba6197b0b5ceefb6518836f14ef65f2:Assets/Scripts/Test/UnitTest/UnitActionSystemUI.cs
            foreach (BaseAction baseAction in selectunit.GetBaseActionArray())
            {
                Transform actionButtonTransform = Instantiate(actionButtonPrefab, actionButtonContainerTransform);
                ActionButtonUI actionButtonUI = actionButtonTransform.GetComponent<ActionButtonUI>();
                actionButtonUI.SetBaseAction(baseAction);

                actionButtonUIList.Add(actionButtonUI);
            }
        }
        private void UnitActionSystem_OnSelectedUnitChanged(object sender, EventArgs e)
        {
            CreateUnitActionButtons();
            UpdateSelectedVisual();
            UpdateActionPoints();
        }

        private void UnitActionSystem_OnSelectedActionChanged(object sender, EventArgs e)
        {
            UpdateSelectedVisual();
        }

        private void UnitActionSystem_OnActionStarted(object sender, EventArgs e)
        {
            UpdateActionPoints();
        }
        private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
        {
            UpdateActionPoints();
        }
        private void Unit_OnAnyActionPointsChanged(object sender, EventArgs e)
        {
            UpdateActionPoints();
        }

        private void UpdateSelectedVisual()
        {
            foreach (ActionButtonUI actionButtonUI in actionButtonUIList)
            {
                actionButtonUI.UpdateSelectedVisual();
            }
        }
        private void UpdateActionPoints()
        {
            UnitTest selectedUnit = unitActionSystem.GetSelectedUnit();

            actionPointsText.text = "Action Points: " + selectedUnit.GetActionPoints();
        }
    }
}
