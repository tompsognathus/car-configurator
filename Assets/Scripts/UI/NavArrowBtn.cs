using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Button used to navigate between the different items shown on screen such 
/// as different cars or different accessories
/// </summary>
public class NavArrowBtn : MonoBehaviour
{
    public void SetInteractable(bool state)
    {
        GetComponent<Button>().interactable = state;
    }
}
