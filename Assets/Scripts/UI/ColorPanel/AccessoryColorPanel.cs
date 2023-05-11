using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class is used to manage a set of toggle buttons that can be used to
/// change the colors of an accessory.
/// </summary>
public class AccessoryColorPanel : ColorPanel
{
    void OnEnable()
    {
        // Ensure toggles are updated when switching between accessories
        EventManager.Instance.AccessoryFeaturedEvent += UpdateToggleStates;
        // and when the color selector UI screen is opened
        UpdateToggleStates();
    }

    void OnDisable()
    {
        // Technically redundant as toggle states only get updated when this
        // panel is enabled, but included to avoid future errors if functionality
        // changes.
        EventManager.Instance.AccessoryFeaturedEvent -= UpdateToggleStates;
    }

    /// <summary>
    /// Used to match the set of toggles to whatever the current active color
    /// of the featured accessory is. Called when switching between accessories
    /// or to a color selector UI screen, otherwise the toggles may remain
    /// set to a previous color.
    /// </summary>
    void UpdateToggleStates()
    {
        int accessoryIdx = StageManager.SelectedCar.FeaturedAccessorySlotIdx;
        bool accessoryIsSelected = StageManager.SelectedCar.AvailableAccessorySlots[accessoryIdx].IsSelected;

        CarColorMaterial featuredAccessoryColor = StageManager.SelectedCar.AvailableAccessorySlots[accessoryIdx].CurrentColor;
        SelectToggle(featuredAccessoryColor);
    }

    /// <summary>
    /// Selects the toggle that matches the given color or selects the 
    /// 'Accessory Deselected' toggle if the given color is not found.
    /// </summary>
    public void SelectToggle(CarColorMaterial color)
    {
        bool colorMatched = false;
        foreach (Toggle toggle in toggleList)
        {
            if (
                toggle.GetComponent<ColorSelectToggle>() != null
                && toggle.GetComponent<ColorSelectToggle>().CarColor == color
            )
            {
                toggle.isOn = true;
                colorMatched = true;
                break;
            }
        }
        if (!colorMatched)
        {   
            GetComponentsInChildren<Toggle>()[0].isOn = true;
        }
    }
}
