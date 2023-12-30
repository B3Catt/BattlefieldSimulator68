using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BattlefieldSimulator;

public class UnitDetailsUI : MonoBehaviour
{
    public static UnitDetailsUI Instance { get; private set; }
    [SerializeField] private TextMeshProUGUI UnitName;
    [SerializeField] private TextMeshProUGUI UnitAttackDistance;
    [SerializeField] private TextMeshProUGUI UnitMoveDistance;
    [SerializeField] private TextMeshProUGUI UnitAttackPower;
    [SerializeField] private TextMeshProUGUI UnitDetails;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There's more than one UnitDetailsUI! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    private void Start()
    {
        gameObject.SetActive(false);
        UnitActionSystem.Instance.OnSelectedUnitChange += UnitActionSystem_OnSelectedUnitChanged;
    }

    private void UnitActionSystem_OnSelectedUnitChanged(object sender, EventArgs e)
    {
        UpdateDetailsUI();
    }
    public void UpdateDetailsUI()
    {
        UnitTest unitTest = UnitActionSystem.Instance.GetSelectedUnit();
        if (unitTest == null)
        {
            gameObject.SetActive(false);
            return;
        }
        UnitName.text = "部队名 : " + unitTest.unitname;
        UnitAttackDistance.text = "部队攻击距离 : " + unitTest.attackdistance;
        UnitMoveDistance.text = "部队移动距离 : " + unitTest.movedistance;
        UnitAttackPower.text = "部队攻击力 : " + unitTest.attackpower;
        UnitDetails.text = "部队详情 : " + unitTest.UnitDetails;
        gameObject.SetActive(true);
    }
}
