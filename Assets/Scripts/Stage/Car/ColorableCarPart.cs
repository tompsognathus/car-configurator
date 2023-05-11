using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Attach this component to car parts you want to be colorable by the user. 
/// At the moment, functionality is limited to just one car part that gets set
/// in the car color selector. Note that this can have as many components as you
/// like, but they will all be set to the same color.
/// 
/// TODO - allow for multiple colorable car parts the user can toggle between using selector buttons
/// </summary>
public class ColorableCarPart : MonoBehaviour
{
    [field: SerializeField] public CarColorMaterial DefaultColor { get; private set; }
    [field: SerializeField] public ColorableCarPartComponent[] ColorableComponents { get; private set; }
    [field: SerializeField] public CarColorMaterial CurrentColor { get; private set; }

    void Start()
    {
        SetDefaultColor();
    }

    void OnEnable()
    {
        EventManager.Instance.StageResetEvent += SetDefaultColor;
    }

    void OnDisable()
    {
        EventManager.Instance.StageResetEvent -= SetDefaultColor;
    }

    /// <summary>
    /// Sets the color of all colorable components to the given color. Currently
    /// only used on the car color selector canvas.
    /// </summary>
    /// <param name="newColor"></param> The color to set all colorable components to
    public void SetColor(CarColorMaterial newColor)
    {
        CurrentColor = newColor;
        foreach (ColorableCarPartComponent colorableComponent in ColorableComponents)
        {
            colorableComponent.GetComponent<Renderer>().material.color = newColor.material.color;
        }
    }

    void SetDefaultColor()
    {
        foreach (ColorableCarPartComponent colorableComponent in ColorableComponents)
        {
            SetColor(DefaultColor);
        }
    }
}
