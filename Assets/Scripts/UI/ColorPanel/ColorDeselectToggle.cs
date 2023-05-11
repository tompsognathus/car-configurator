using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorDeselectToggle : MonoBehaviour
{
    void Start()
    {
        // Run HandleToggleClick whenever the toggle is clicked
        GetComponent<Toggle>().onValueChanged.AddListener(delegate { HandleToggleClick(); });
    }
    void OnDestroy()
    {
        // Unregister the listener when the toggle is destroyed
        GetComponent<Toggle>().onValueChanged.RemoveListener(delegate { HandleToggleClick(); });
    }

    void HandleToggleClick()
    {
        // We only want to react if the toggle was switched on, to avoid events
        // being fired twice when a different toggle is selected and this one
        // is deselected

        if (GetComponent<Toggle>().isOn)
        {
            EventManager.Instance.InvokeRemoveAccessoryEvent();
        }
    }
}
