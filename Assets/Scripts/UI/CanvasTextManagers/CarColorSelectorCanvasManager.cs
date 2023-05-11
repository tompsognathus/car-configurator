using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Takes care of updating the UI on the car color selector canvas.
/// </summary>
public class CarColorSelectorCanvasManager : CanvasUpdateManager
{
    void OnEnable()
    {
        EventManager.Instance.RequestUIUpdateEvent += SetModel;
        EventManager.Instance.UICanvasChangedEvent += SetModel;
    }

    void OnDisable()
    {
        EventManager.Instance.RequestUIUpdateEvent -= SetModel;
        EventManager.Instance.UICanvasChangedEvent -= SetModel;
    }

    /// <summary>
    /// Toggles whether nav buttons are active or not.
    /// Currently unused as there aren't any nav buttons on this canvas since we 
    /// can't set the color of different car parts independently.
    /// TODO: Use this method after implementing the ability to do so.
    /// </summary>
    void SetNavBtnVisibility(bool isVisible)
    {
        foreach (NavArrowBtn btn in GetComponentsInChildren<NavArrowBtn>())
        {
            btn.gameObject.SetActive(isVisible);
        }
    }
}
