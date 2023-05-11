using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Takes care of updating the UI on the review order canvas.
/// </summary>
public class ReviewOrderCanvasManager : CanvasUpdateManager
{
    [SerializeField] TextMeshProUGUI selectedVehicleModelText;
    [SerializeField] TextMeshProUGUI selectedVehiclePriceText;

    [SerializeField] TextMeshProUGUI accessoriesItemsText;
    [SerializeField] TextMeshProUGUI accessoriesPriceText;
    [SerializeField] RectTransform orderSummaryViewport;

    void OnEnable()
    {
        SetPrice();
        SetUpBrandAndModelItem();
        SetUpAccessoryItems();

        // Update the price when the UI is changed
        EventManager.Instance.UICanvasChangedEvent += SetPrice;

        // But also allow other components to update the price through the UIManager
        EventManager.Instance.RequestUIUpdateEvent += SetPrice;
    }

    void OnDisable()
    {
        EventManager.Instance.RequestUIUpdateEvent -= SetPrice;
        EventManager.Instance.UICanvasChangedEvent -= SetPrice;
    }

    /// <summary>
    /// populate the brand and model text as well as its price
    /// </summary>
    void SetUpBrandAndModelItem()
    {
        selectedVehicleModelText.text = StageManager.SelectedCar.GetBrandAndModel();
        selectedVehiclePriceText.text = UIManager.CurrencySymbol + StageManager.SelectedCar.GetBaseCost().ToString("N0");
    }

    /// <summary>
    /// Loop through all the selected accessories and populate the accessory items text as well as its price line by line
    /// </summary>
    void SetUpAccessoryItems()
    {
        int numSelectedAccessories = 0;
        string accessoriesItemsTextCandidate = "";
        string accessoriesPriceTextCandidate = "";
        foreach (AccessorySlot accessorySlot in StageManager.SelectedCar.AvailableAccessorySlots)
        {
            if (accessorySlot.IsSelected)
            {
                numSelectedAccessories++;

                accessoriesItemsTextCandidate += accessorySlot.Accessory.AccessoryName + "\n";
                accessoriesPriceTextCandidate += UIManager.CurrencySymbol + accessorySlot.Accessory.Price.ToString("N0") + "\n";
            }
        }

        if (numSelectedAccessories > 0)
        {
            accessoriesItemsText.text = accessoriesItemsTextCandidate;
            accessoriesPriceText.text = accessoriesPriceTextCandidate;
        }
        else
        {
            accessoriesItemsText.text = "No accessories selected";
            accessoriesPriceText.text = "";
        }
        if (StageManager.SelectedCar.AvailableAccessorySlots.Count == 0)
        {
            accessoriesItemsText.text = "No accessories available";
            accessoriesPriceText.text = "";
        }
    }
}
