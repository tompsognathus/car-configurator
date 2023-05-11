using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorSelectToggle : MonoBehaviour
{
    [Tooltip("Sets the color of the button at runtime and is used to determine which color gets applied to car or accessory")]
    [field: SerializeField] public CarColorMaterial CarColor { get; private set;}

    void Start()
    {
        // Run HandleToggleClick whenever the toggle is clicked
        GetComponent<Toggle>().onValueChanged.AddListener(delegate { HandleToggleClick(); });
    }

    /// <summary>
    /// Unity has no OnToggleTurnedOn listener, so we're mimicking the function
    /// of what that would be. We use a group of toggles to select our colors,
    /// with only one ever being active at a given time. When a toggle is activated,
    /// we want to treat this as the color being selected, and ignore any other
    /// toggles being deselected at the same time.
    /// </summary>
    void HandleToggleClick()
    {
        if (GetComponent<Toggle>().isOn)
        {
            bool isInAccessoryColorPanel = GetComponentInParent<AccessoryColorPanel>() != null;
            bool isInCarColorPanel = GetComponentInParent<CarColorPanel>() != null;

            if (isInAccessoryColorPanel)
            {
                EventManager.Instance.InvokeAccessoryColorSelectedEvent(CarColor);
            }
            else if (isInCarColorPanel)
            {
                EventManager.Instance.InvokeCarColorSelectedEvent(CarColor);
            }
        }
    }
}
