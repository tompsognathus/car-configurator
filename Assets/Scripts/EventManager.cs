using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance { get; private set; }
    [SerializeField] UIManager uiManager;

    /// <summary>
    /// 
    /// </summary>
    public event Action RequestUIUpdateEvent;

    /// <summary>
    /// 
    /// </summary>
    public event Action UICanvasChangedEvent;

    /// <summary>
    /// Set your code up so that calling this event reverts things to how you
    /// want them to be whenever the user is starting a new car configuration.
    /// </summary>
    public event Action StageResetEvent;

    /// <summary>
    /// Called when the user selects a color from the color bar on the car color
    /// selector page
    /// </summary>
    public event Action<CarColorMaterial> CarColorSelectedEvent;

    public event Action CarSelectedEvent;

    /// <summary>
    /// Called as the user cycles through accessories
    /// </summary>
    public event Action AccessoryFeaturedEvent;

    /// <summary>
    /// Called when the user selects a color from the color bar on the accessory
    /// selector page
    /// </summary>
    public event Action<CarColorMaterial> AccessoryColorSelectedEvent;

    /// <summary>
    /// Called when the user clicks the "X" button on the accessory selector page
    /// </summary>
    public event Action RemoveAccessoryEvent;

    private void Awake()
    {
        // Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        uiManager.DisableAllUICanvases();
    }

    public void InvokeRequestUIUpdate()
    {
        // Debug.Log("OnRequestUIUpdate Invoked");
        StartCoroutine(InvokeDelayedEvent(RequestUIUpdateEvent));
    }

    public void InvokeUICanvasChanged()
    {
        // Debug.Log("OnUICanvasChanged Invoked");
        StartCoroutine(InvokeDelayedEvent(UICanvasChangedEvent));
    }

    public void InvokeStageReset()
    {
        // Debug.Log("OnStageReset Invoked");
        StartCoroutine(InvokeDelayedEvent(StageResetEvent));
    }

    public void InvokeCarColorSelectedEvent(CarColorMaterial carColor)
    {
        // Debug.Log("OnCarColorSelected Invoked");
        StartCoroutine(InvokeDelayedEvent(CarColorSelectedEvent, carColor));
    }

    public void InvokeCarSelectedEvent(Car car)
    {
        // Debug.Log("OnCarSelected Invoked");
        StartCoroutine(InvokeDelayedEvent(CarSelectedEvent));
    }

    public void InvokeAccessoryColorSelectedEvent(CarColorMaterial carColor)
    {
        // Debug.Log("OnAccessoryColorSelected Invoked");
        StartCoroutine(InvokeDelayedEvent(AccessoryColorSelectedEvent, carColor));
    }

    public void OnAccessoryFeatured()
    {
        // Debug.Log("OnAccessoryFeatured Invoked");
        StartCoroutine(InvokeDelayedEvent(AccessoryFeaturedEvent));
    }

    public void InvokeRemoveAccessoryEvent()
    {
        // Debug.Log("OnRemoveAccessory Invoked");
        StartCoroutine(InvokeDelayedEvent(RemoveAccessoryEvent));
    }

    // We delay by a frame to ensure everything is set up (activated, deactivated)
    // TODO verify whether this is actually necessary or whether this was a side effect of trying to fix a bug caused by something else
    IEnumerator InvokeDelayedEvent(Action action)
    {
        yield return new WaitForEndOfFrame();
        action?.Invoke();
    }
    IEnumerator InvokeDelayedEvent(Action<CarColorMaterial> action, CarColorMaterial carColor)
    {
        yield return new WaitForEndOfFrame();
        action?.Invoke(carColor);
    }
}
