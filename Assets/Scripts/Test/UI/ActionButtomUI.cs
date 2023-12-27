using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BattlefieldSimulator;

public class ActionButtonUI : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI textMeshPro;
    [SerializeField] private Button button;
    [SerializeField] private GameObject selectedGameObject;


    public BaseAction baseAction;

    public void SetBaseAction(BaseAction baseAction)
    {
        this.baseAction = baseAction;
        textMeshPro.text = baseAction.GetActionName().ToUpper();

        // button.onClick.AddListener(() => {
        //     UnitActionSystem.Instance.SetSelectedAction(baseAction);
        // });
    }

    // public void UpdateSelectedVisual()
    // {
    //     BaseAction selectedBaseAction = UnitActionSystem.Instance.GetSelectedAction();
    //     selectedGameObject.SetActive(selectedBaseAction == baseAction);
    // }

}
