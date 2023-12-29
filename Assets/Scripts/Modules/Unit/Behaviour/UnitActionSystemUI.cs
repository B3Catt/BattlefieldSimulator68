using System.Collections;
using System;
using System.Collections.Generic;
using BattlefieldSimulator;
using UnityEngine;
using UnityEngine.UI;
namespace BattlefieldSimulator
{

    public class UnitActionSystemUI : MonoBehaviour
    {
        public UnitActionSystem unitActionSystem;
        [SerializeField] private Transform actionButtonPrefab;

        [SerializeField] private Transform actionButtonContainerTransform;
        private void Start()
        {
            UnitActionSystem.Instance.OnSelectedUnitChange += UnitActionSystem_OnSelectedUnitChanged;
            CreateUnitActionButtons();
        }
        private void CreateUnitActionButtons()
        {
            foreach (Transform buttonTransform in actionButtonContainerTransform)
            {
                Destroy(buttonTransform.gameObject);
            }
            Unit selectunit = unitActionSystem.GetSelectedUnit();
            foreach (BaseAction baseAction in selectunit.GetBaseActionArray())
            {
                Transform actionButtonTransform = Instantiate(actionButtonPrefab, actionButtonContainerTransform);
                ActionButtonUI actionButtonUI = actionButtonTransform.GetComponent<ActionButtonUI>();
                actionButtonUI.SetBaseAction(baseAction);
            }
        }
        private void UnitActionSystem_OnSelectedUnitChanged(object sender, EventArgs e)
        {
            CreateUnitActionButtons();
        }
    }
}
