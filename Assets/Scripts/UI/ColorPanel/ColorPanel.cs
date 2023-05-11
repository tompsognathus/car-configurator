using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class ColorPanel : MonoBehaviour
{
    [field: SerializeField] protected StageManager StageManager { get; private set; }
    [field: SerializeField] protected List<Toggle> toggleList { get; private set; }


    protected void Start()
    {
        SetUpToggleColors();
    }

    /// <summary>
    /// Sets up the color of each toggle based on values set in the inspector. 
    /// This is is done to ensure color buttons match car colors precisely, as 
    /// well as to avoid having buttons switch to a different color on selection
    /// etc.
    /// </summary>
    void SetUpToggleColors()
    {
        foreach (Toggle colorToggle in toggleList)
        {
            if (colorToggle.GetComponent<ColorSelectToggle>() == null) continue; // The deselect button has an image instead of a color, so we skip it

            ColorBlock toggleColors = colorToggle.colors;
            toggleColors.normalColor = colorToggle.GetComponent<ColorSelectToggle>().CarColor.material.color;
            toggleColors.highlightedColor = toggleColors.normalColor;
            toggleColors.pressedColor = toggleColors.normalColor;
            toggleColors.selectedColor = toggleColors.normalColor;

            colorToggle.colors = toggleColors;
        }
    }

}
