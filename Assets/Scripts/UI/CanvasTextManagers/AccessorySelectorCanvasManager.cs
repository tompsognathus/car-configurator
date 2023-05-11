using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Manages the accessory selector canvas, ensuring all elements get updated
/// as necessary.
/// </summary>
public class AccessorySelectorCanvasManager : CanvasUpdateManager
{
    // Arrows used to cycle through accessories
    [Tooltip("A button the user can click to cycle to the next accessory")]
    [SerializeField] NavArrowBtn nextAccessoryBtn;
    [Tooltip("A button the user can click to cycle to the previous accessory")]
    [SerializeField] NavArrowBtn prevAccessoryBtn;

    private void Awake()
    {
        // Start of with the arrows disabled, in case we only have one accessory
        SetNavArrowInteractability(false);
    }

    void OnEnable()
    {
        EventManager.Instance.RequestUIUpdateEvent += SetPrice;
        EventManager.Instance.RequestUIUpdateEvent += SetPriceChange;
        EventManager.Instance.RequestUIUpdateEvent += SetModel;
        EventManager.Instance.RequestUIUpdateEvent += SetAccessoryName;
        EventManager.Instance.RequestUIUpdateEvent += SetAccessoryDescription;

        EventManager.Instance.UICanvasChangedEvent += SetPrice;
        EventManager.Instance.UICanvasChangedEvent += SetPriceChange;
        EventManager.Instance.UICanvasChangedEvent += SetModel;
        EventManager.Instance.UICanvasChangedEvent += SetAccessoryName;
        EventManager.Instance.UICanvasChangedEvent += SetAccessoryDescription;

        EventManager.Instance.AccessoryFeaturedEvent += SetPrice;
        EventManager.Instance.AccessoryFeaturedEvent += SetPriceChange;
        EventManager.Instance.AccessoryFeaturedEvent += SetAccessoryName;
        EventManager.Instance.AccessoryFeaturedEvent += SetAccessoryDescription;

        SetPrice();  // Always update price when this component is enabled

        // Always start by showing the first accessory slot, regardless of
        // which one is currently selected or which one the user last viewed
        StageManager.SelectedCar.ShowFirstAccessorySlot();

        // Only activate nav arrows if we have multiple accessories to choose from
        if (StageManager.SelectedCar.AvailableAccessorySlots.Count > 1)
        {
            SetNavArrowInteractability(true);
        }
    }

    void OnDisable()
    {
        // If an accessory is currently featured and disabled, it appears as
        // semi-transparent. If the user moves to another canvas, we want to
        // Hide it altogether and return it to the accessory pool, so that
        // only the selected accessories remain visible and attached to the car.
        StageManager.SelectedCar.HideDisabledAccessories();

        EventManager.Instance.RequestUIUpdateEvent -= SetPrice;
        EventManager.Instance.RequestUIUpdateEvent -= SetPriceChange;
        EventManager.Instance.RequestUIUpdateEvent -= SetModel;
        EventManager.Instance.RequestUIUpdateEvent -= SetAccessoryName;
        EventManager.Instance.RequestUIUpdateEvent -= SetAccessoryDescription;

        EventManager.Instance.UICanvasChangedEvent -= SetPrice;
        EventManager.Instance.UICanvasChangedEvent -= SetPriceChange;
        EventManager.Instance.UICanvasChangedEvent -= SetModel;
        EventManager.Instance.UICanvasChangedEvent -= SetAccessoryName;
        EventManager.Instance.UICanvasChangedEvent -= SetAccessoryDescription;

        EventManager.Instance.AccessoryFeaturedEvent -= SetPrice;
        EventManager.Instance.AccessoryFeaturedEvent -= SetPriceChange;
        EventManager.Instance.AccessoryFeaturedEvent -= SetAccessoryName;
        EventManager.Instance.AccessoryFeaturedEvent -= SetAccessoryDescription;

        SetNavArrowInteractability(false);
    }

    /// <summary>
    /// Toggle whether the nav arrows can be used or are greyed out.
    /// </summary>
    void SetNavArrowInteractability(bool state)
    {
        nextAccessoryBtn.SetInteractable(state);
        prevAccessoryBtn.SetInteractable(state);
    }
}
