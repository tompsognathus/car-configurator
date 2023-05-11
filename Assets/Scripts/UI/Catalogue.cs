using System.ComponentModel;
using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// Takes care of updating the UI on the catalogue canvas. The catalogue canvas
/// is designed to display a list of all available cars, accessories and their
/// respective prices. The canvas is populated automatically at runtime based on 
/// the cars and accessories set up in the project
/// </summary>
public class Catalogue : MonoBehaviour
{
    [SerializeField] StageManager stageManager;
    [SerializeField] UIManager uiManager;
    [SerializeField] Transform cataloguePanel;
    [SerializeField] TextMeshProUGUI itemNames;
    [SerializeField] TextMeshProUGUI itemPrices;

    void Start()
    {
        DeactivateCatalogue();
    }

    public void ActivateCatalogue()
    {
        cataloguePanel.gameObject.SetActive(true);
        // Populating catalogue on click rather than on start because some of
        // the required info such as the list of available accessories for
        // each car may not be available yet.
        PopulateCatalogue();
        // UpdateContentSizeFitter();
    }

    public void DeactivateCatalogue()
    {
        cataloguePanel.gameObject.SetActive(false);
    }

    /// <summary>
    /// Iterates through all the cars and their available accessories in the
    /// project scene and populates the catalogue with their respective prices.
    /// </summary>
    void PopulateCatalogue()
    {
        itemNames.text = "";
        itemPrices.text = "";

        foreach (Car car in stageManager.GetAllCars())
        {
            itemNames.text += car.GetBrandAndModel() + "\n";
            itemPrices.text += uiManager.CurrencySymbol + car.GetPriceWithSelectedAccessories().ToString("N0") + "\n";

            if (car.AvailableAccessorySlots.Count == 0)
            {
                itemNames.text += "        No accessories available\n";
                itemPrices.text += "-\n";
            }
            else
            {
                foreach (AccessorySlot accessorySlot in car.AvailableAccessorySlots)
                {
                    Accessory accessory = accessorySlot.Accessory;

                    itemNames.text += "        " + accessory.AccessoryName + "\n";
                    itemPrices.text += uiManager.CurrencySymbol + accessory.Price.ToString("N0") + "\n";

                }
            }
        }
    }
}
