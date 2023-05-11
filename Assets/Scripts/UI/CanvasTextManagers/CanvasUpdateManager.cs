using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// Contains general variables and methods for updating the UI that get used by
/// multiple canvas managers (or may do so in the future).
/// </summary>
public abstract class CanvasUpdateManager : MonoBehaviour
{
    [field: SerializeField] protected StageManager StageManager { get; private set; }
    [field: SerializeField] protected UIManager UIManager { get; private set; }

    [field: SerializeField] protected TextMeshProUGUI CarBrandAndModelText { get; private set; }
    [field: SerializeField] protected TextMeshProUGUI CarPriceText { get; private set; }
    [field: SerializeField] protected TextMeshProUGUI PriceChangeText { get; private set; }
    [field: SerializeField] protected TextMeshProUGUI AccessoryNameText { get; private set; }
    [field: SerializeField] protected TextMeshProUGUI AccessoryDescriptionText { get; private set; }
    [field: SerializeField] protected ColorPanel ColorPanel { get; private set; }
    [field: SerializeField] protected SpecsPanel SpecsPanel { get; private set; }

    protected void SetPrice()
    {
        int price = StageManager.SelectedCar.GetPriceWithSelectedAccessories();
        CarPriceText.text = UIManager.CurrencySymbol + price.ToString("N0");
    }

    protected void SetPriceChange()
    {
        int priceChange = StageManager.SelectedCar.GetSelectedAccessoryPrice();
        bool accessoryIsSelected = StageManager.SelectedCar.GetAccessorySelectedState();
        if (!accessoryIsSelected)
        {
            PriceChangeText.text = "+" + priceChange.ToString("N0");
        }
        else
        {
            PriceChangeText.text = "";
        }
    }

    protected void SetModel()
    {
        string modelText = StageManager.SelectedCar.GetBrandAndModel();
        CarBrandAndModelText.text = modelText;
    }

    protected void SetAccessoryName()
    {
        string accessoryName = StageManager.SelectedCar.GetSelectedAccessoryName();
        AccessoryNameText.text = accessoryName;
    }

    protected void SetAccessoryDescription()
    {
        string accessoryDescription = StageManager.SelectedCar.GetSelectedAccessoryDescription();
        AccessoryDescriptionText.text = accessoryDescription;
    }

    protected void UpdateSpecsPanel()
    {
        int topSpeed = StageManager.SelectedCar.TopSpeed;
        int acceleration = StageManager.SelectedCar.Acceleration;
        int handling = StageManager.SelectedCar.Handling;

        SpecsPanel.SetSliderValues(topSpeed, acceleration, handling);
    }
}
