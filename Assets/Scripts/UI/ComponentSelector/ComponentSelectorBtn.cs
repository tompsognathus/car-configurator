using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// TODO
/// Currently unused. The basic car model has experimental component selectors set up
/// but disabled. The idea was to allow the player to select different components to
/// change their color, but this was not fully implemented due to time constraints.
/// </summary>
public class ComponentSelectorBtn : MonoBehaviour
{
    [field: SerializeField] public List<GameObject> ComponentList { get; private set; }

    ComponentSelector componentSelector;

    void Start()
    {
        componentSelector = GetComponentInParent<ComponentSelector>();
    }

    public void HighlightSelector()
    {
        UnhighlightAllSelectors();
        GetComponentInChildren<Image>().color = Color.yellow;
    }

    public void UnhighlightAllSelectors()
    {
        foreach (Button btn in componentSelector.componentBtnList)
        {
            btn.GetComponentInChildren<Image>().color = Color.white;
        }
    }
    
}
