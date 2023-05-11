using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// TODO
/// Currently unused. The basic car model has experimental component selectors set up
/// but disabled. The idea was to allow the player to select different components to 
/// change their color, but this was not fully implemented due to time constraints.
/// </summary>
public class ComponentSelector : MonoBehaviour
{
    public List<Button> componentBtnList { get; private set; }

    void Start()
    {
        componentBtnList = FetchBtnList();
    }

    /// <summary>
    /// Creates a list of all the available part selectors.
    /// </summary>
    List<Button> FetchBtnList()
    {
        List<Button> componentBtnList = new List<Button>();
        foreach (Button btn in transform.GetComponentsInChildren<Button>())
        {
            componentBtnList.Add(btn);
        }

        return componentBtnList;
    }

}
