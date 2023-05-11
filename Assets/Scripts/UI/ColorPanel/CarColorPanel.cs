using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class is used to manage a set of toggle buttons that can be used to
/// change the colors of a vehicle.
/// </summary>
public class CarColorPanel : ColorPanel
{
    void OnEnable()
    {
        /// Ensure toggles are updated whenever the car color selector UI is
        /// opened.
        UpdateToggleStates();
    }

    /// <summary>
    /// Used to match the set of toggles to whatever the current active color
    /// of the selected car is. Called when switching to a color selector UI 
    /// screen, otherwise the toggles may remain set to a previous color.
    /// </summary>
    void UpdateToggleStates()
    {
        CarColorMaterial carColor = StageManager.SelectedCar.SelectedCarPart.CurrentColor;
        SelectToggle(carColor);
    }

    /// <summary>
    /// Selects the toggle that matches the given color.
    /// </summary>
    public void SelectToggle(CarColorMaterial color)
    {
        foreach (Toggle colorToggle in toggleList)
        {
            if (
                colorToggle.GetComponent<ColorSelectToggle>() != null
                && colorToggle.GetComponent<ColorSelectToggle>().CarColor == color
            )
            {
                colorToggle.isOn = true;
                break;
            }
        }
    }
}
