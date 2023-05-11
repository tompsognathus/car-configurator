using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Takes care of updating the UI on the car selector canvas.
/// </summary>
public class CarSelectorCanvasManager : CanvasUpdateManager
{
    [SerializeField] NavArrowBtn nextCarBtn;
    [SerializeField] NavArrowBtn prevCarBtn;

    private void Awake() {
        // Start off with the nav arrows disabled in case we only have one car
        SetNavArrowsInteractability(false);
    }

    void OnEnable()
    {
        EventManager.Instance.RequestUIUpdateEvent += SetPrice;
        EventManager.Instance.RequestUIUpdateEvent += SetModel;
        EventManager.Instance.RequestUIUpdateEvent += UpdateSpecsPanel;

        EventManager.Instance.UICanvasChangedEvent += SetPrice;
        EventManager.Instance.UICanvasChangedEvent += SetModel;
        
        // Reset the stage when we enable this canvas. Basically, whenever the
        // user returns to this canvas, we want to start from scratch since they're
        // choosing a car model as opposed to configuring it
        EventManager.Instance.InvokeStageReset();
        
        UpdateSpecsPanel();
        SetPrice();

        // Only activate nav arrows if we have multiple cars to choose from
        if (StageManager.NumCars > 0)
        {
            SetNavArrowsInteractability(true);
        }
    }

    void OnDisable()
    {
        EventManager.Instance.RequestUIUpdateEvent -= SetPrice;
        EventManager.Instance.RequestUIUpdateEvent -= SetModel;
        EventManager.Instance.RequestUIUpdateEvent -= UpdateSpecsPanel;

        EventManager.Instance.UICanvasChangedEvent -= SetPrice;
        EventManager.Instance.UICanvasChangedEvent -= SetModel;

        SetNavArrowsInteractability(false);
    }

    /// <summary>
    /// Toggle whether the nav arrows can be used or are greyed out.
    /// </summary>
    void SetNavArrowsInteractability(bool state)
    {
        nextCarBtn.SetInteractable(state);
        prevCarBtn.SetInteractable(state);
    }
}
