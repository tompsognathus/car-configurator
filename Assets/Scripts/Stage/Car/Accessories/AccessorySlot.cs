using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An accessory slot is used to hold an accessory. Attached to an empty
/// gameobject, its transform is used to position, scale and rotate the accessory
/// to fit any given car. This allows us to have a single accessory pool with
/// only one copy of each accessory, which is then used for all cars.
/// </summary>
public class AccessorySlot : MonoBehaviour
{
    [field: SerializeField] public Accessory Accessory { get; private set; }
    [field: SerializeField] public CarColorMaterial CurrentColor { get; private set; }
    [field: SerializeField] public bool IsSelected { get; private set; }
    public int SelectedColorBtnIdx { get; private set; }

    [SerializeField] UIManager uiManager;
  
    void Start()
    {
        // All accessories should be deselected whenever the stage is reset.
        EventManager.Instance.StageResetEvent += Deselect;
        // And also when the stage is initially set up
        Deselect();
    }

    /// <summary>
    /// Redundant at the moment as we never destroy cars or accessories, but
    /// may come into play if we were to e.g. expand the car color selector into
    /// a full game with different scenes etc.
    /// </summary>
    void OnDestroy()
    {
        EventManager.Instance.StageResetEvent -= Deselect;
    }

    /// <summary>
    /// Used to control whether an accessory slot should react to color selection
    /// UI clicks. This is used to prevent slots from being selected when
    /// they are not featured in the UI.
    /// </summary>
    /// <param name="isFeatured"></param> boolean indicating whether the accessory
    /// is currently being featured in the UI.
    public void OnFeaturedOrUnfeatured(bool isFeatured)
    {
        if (isFeatured)
        {
            EventManager.Instance.AccessoryColorSelectedEvent += Select;
            EventManager.Instance.RemoveAccessoryEvent += Deselect;
        }
        else
        {
            EventManager.Instance.AccessoryColorSelectedEvent -= Select;
            EventManager.Instance.RemoveAccessoryEvent -= Deselect;
        }
    }

    /// <summary>
    /// Used to select an accessory (and its color)
    /// </summary>
    /// <param name="color"></param> the color to set the accessory to
    void Select(CarColorMaterial color)
    {
        IsSelected = true;
        // Set the accessory's color
        Accessory.SetColorableColor(color);
        CurrentColor = color;
        // Need a price update
        EventManager.Instance.InvokeRequestUIUpdate();
    }

    /// <summary>
    /// Used to deselect an accessory (and make it semi-transparent)
    /// </summary>
    void Deselect()
    {
        IsSelected = false;
        // Make the accessory semi-transparent
        Accessory.SetAllPartsColor(uiManager.DeselectedAccessoryColor);
        CurrentColor = uiManager.DeselectedAccessoryColor;
        // Need a price update
        EventManager.Instance.InvokeRequestUIUpdate();
    }
}
