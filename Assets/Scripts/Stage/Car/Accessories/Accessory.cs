using System.Security;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Accessory : MonoBehaviour
{
    [field: SerializeField] public string AccessoryName { get; private set; }
    [field: SerializeField] public string AccessoryDescription { get; private set; }
    [field: SerializeField] public int Price { get; private set; }
    [field: SerializeField] List<Transform> ColorableParts;
    [field: SerializeField] List<Transform> AllParts;

    /// <summary>
    /// Sets the color of any elements listed as colorable in the inspector to 
    /// a given color.
    /// </summary>
    /// <param name="color"></param>
    public void SetColorableColor(CarColorMaterial color)
    {
        SetDefaultColors();
        foreach (Transform component in ColorableParts)
        {
            component.GetComponent<Renderer>().material = color.material;
        }
    }

    /// <summary>
    /// Sets the color of all parts of the accessory to a given color. This is 
    /// only really used to make the accessory semi-transparent when it is
    /// deselected.
    /// </summary>
    /// <param name="color"></param>
    public void SetAllPartsColor(CarColorMaterial color)
    {
        foreach (Transform component in AllParts)
        {
            component.GetComponent<Renderer>().material = color.material;
        }
    }

    /// <summary>
    /// Resets the color of all parts of the accessory to their default colors
    /// as defined in the inspector.
    /// </summary>
    void SetDefaultColors()
    {
        foreach (Transform accessoryPart in AllParts)
        {
            accessoryPart.GetComponent<Renderer>().material = accessoryPart.GetComponent<AccessoryPart>().DefaultMaterial;
        }
    }
}
