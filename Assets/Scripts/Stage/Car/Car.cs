using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    [field: Header("On-Stage Car Positioning")]
    [Tooltip("Moves the car closer to or further from the camera")]
    [field: SerializeField] public float DistanceFromCenterStage { get; private set; } = 20f;
    [Tooltip("Determines the car's initial rotation angle")]
    [field: SerializeField] public float CarPresentationAngle { get; private set; } = 120f;
    [Tooltip("Adjusts the car's vertical position (make sure it doesn't float above the stage)")]
    [field: SerializeField] public float HeightAdjustment { get; private set; }

    [field: Header("Car Modification")]
    [Tooltip("Select the car part you want to be changed on the 'Car Color Selector' page. Currently limited to just one.")]
    [field: SerializeField] public ColorableCarPart SelectedCarPart { get; private set; }

    [Header("Car Details")]
    [SerializeField] string brand;
    [SerializeField] string model;
    [SerializeField] int baseCost;

    [Header("Other")]
    [SerializeField] StageManager stageManager;

    [field: Header("Specs")]
    [field: SerializeField, Range(0, 100)] public int TopSpeed { get; private set; }
    [field: SerializeField, Range(0, 100)] public int Acceleration { get; private set; }
    [field: SerializeField, Range(0, 100)] public int Handling { get; private set; }

    public List<AccessorySlot> AvailableAccessorySlots { get; private set; } = new List<AccessorySlot>();
    // Used to track which accessory is being presented in the Accessory Selector
    public int FeaturedAccessorySlotIdx { get; private set; } = 0;

    void Awake()
    {
        // We need to do this before the stage manager starts disabling cars
        // in Start because we need to be able to access the car's available
        // accessory slots from the catalogue
        FetchAvailableAccessorySlots();
    }

    void OnEnable()
    {
        EventManager.Instance.StageResetEvent += HideAllAccessories;
    }

    void OnDisable()
    {
        EventManager.Instance.StageResetEvent -= HideAllAccessories;
    }


    public int GetBaseCost()
    {
        return baseCost;
    }

    public int GetPriceWithSelectedAccessories()
    {
        int configuredPrice = baseCost;

        foreach (AccessorySlot accessorySlot in AvailableAccessorySlots)
            if (accessorySlot.IsSelected)
            {
                configuredPrice += accessorySlot.Accessory.Price;
            }
        return configuredPrice;
    }

    public string GetBrandAndModel()
    {
        return $"{brand} {model}";
    }

    public string GetSelectedAccessoryName()
    {
        return AvailableAccessorySlots[FeaturedAccessorySlotIdx].Accessory.AccessoryName;
    }

    public string GetSelectedAccessoryDescription()
    {
        return AvailableAccessorySlots[FeaturedAccessorySlotIdx].Accessory.AccessoryDescription;
    }

    public int GetSelectedAccessoryPrice()
    {
        return AvailableAccessorySlots[FeaturedAccessorySlotIdx].Accessory.Price;
    }

    public bool GetAccessorySelectedState()
    {
        return AvailableAccessorySlots[FeaturedAccessorySlotIdx].IsSelected;
    }

    /// <summary>
    /// Moves the first accessory from the accessory pool to the car's accessory
    /// slot
    /// </summary>
    public void ShowFirstAccessorySlot()
    {
        HideDisabledAccessories();

        foreach (AccessorySlot accessorySlot in AvailableAccessorySlots)
        {
            accessorySlot.OnFeaturedOrUnfeatured(false);
        }

        FeaturedAccessorySlotIdx = 0;
        AvailableAccessorySlots[FeaturedAccessorySlotIdx].OnFeaturedOrUnfeatured(true);
        ShowFeaturedAccessory(AvailableAccessorySlots[FeaturedAccessorySlotIdx].Accessory, FeaturedAccessorySlotIdx);
    }

    // TODO: Refactor into a single method with a direction parameter
    public void ShowNextAccessorySlot()
    {
        HideDisabledAccessories();

        AvailableAccessorySlots[FeaturedAccessorySlotIdx].OnFeaturedOrUnfeatured(false);
        FeaturedAccessorySlotIdx++;
        FeaturedAccessorySlotIdx = mod(FeaturedAccessorySlotIdx, AvailableAccessorySlots.Count);
        AvailableAccessorySlots[FeaturedAccessorySlotIdx].OnFeaturedOrUnfeatured(true);

        ShowFeaturedAccessory(AvailableAccessorySlots[FeaturedAccessorySlotIdx].Accessory, FeaturedAccessorySlotIdx);
    }

    public void ShowPreviousAccessorySlot()
    {
        HideDisabledAccessories();

        AvailableAccessorySlots[FeaturedAccessorySlotIdx].OnFeaturedOrUnfeatured(false);
        FeaturedAccessorySlotIdx--;
        FeaturedAccessorySlotIdx = mod(FeaturedAccessorySlotIdx, AvailableAccessorySlots.Count);
        AvailableAccessorySlots[FeaturedAccessorySlotIdx].OnFeaturedOrUnfeatured(true);

        ShowFeaturedAccessory(AvailableAccessorySlots[FeaturedAccessorySlotIdx].Accessory, FeaturedAccessorySlotIdx);
    }

    /// <summary>
    /// Moves any accessories that aren't selected to the accessory pool
    /// </summary>
    public void HideDisabledAccessories()
    {
        foreach (AccessorySlot accessorySlot in AvailableAccessorySlots)
        {
            if (!accessorySlot.IsSelected)
            {
                HideAccessory(accessorySlot.Accessory, AvailableAccessorySlots.IndexOf(accessorySlot));
            }
        }
    }

    /// <summary>
    /// Ensures the car's color gets updated when the UI requests it as long as
    /// the car is currently selected.
    /// </summary>
    /// <param name="isSelected"></param>
    public void OnSelectedOrUnselected(bool isSelected)
    {
        if (isSelected)
        {
            EventManager.Instance.CarColorSelectedEvent += SetSelectedCarPartColor;
        }
        else
        {
            EventManager.Instance.CarColorSelectedEvent -= SetSelectedCarPartColor;
        }
    }

    /// <summary>
    /// Returns all accessories to the accessory pool off-screen.
    /// </summary>
    void HideAllAccessories()
    {
        foreach (AccessorySlot accessorySlot in AvailableAccessorySlots)
        {
            HideAccessory(accessorySlot.Accessory, AvailableAccessorySlots.IndexOf(accessorySlot));
        }
    }

    /// <summary>
    /// Attaches an accessory to the car and resets its local position, showing
    /// it on stage. The position, rotation and scale of the accessory is
    /// determined by the accessory slot's transform. This way we can reuse
    /// the same accessory object with different cars.
    /// </summary>
    /// <param name="accessory"></param> the accessory to be shown
    /// <param name="slotIdx"></param> the index of the accessory slot
    void ShowFeaturedAccessory(Accessory accessory, int slotIdx)
    {
        EventManager.Instance.OnAccessoryFeatured();
        // Attach accessory to the car
        accessory.transform.SetParent(AvailableAccessorySlots[FeaturedAccessorySlotIdx].transform);
        // Reset accessory position
        accessory.transform.localPosition = Vector3.zero;
        accessory.transform.localRotation = Quaternion.identity;
        accessory.transform.localScale = Vector3.one;
    }

    /// <summary>
    /// Detaches the accessory in accessorySlot from the car and returns it
    /// to the accessory pool off-screen.
    /// </summary>
    /// <param name="accessorySlot"></param> contains the accessory to be hidden
    /// <param name="slotIdx"></param> the index of the accessory slot
    void HideAccessory(Accessory accessorySlot, int slotIdx)
    {
        // Return accessory to accessory pool
        accessorySlot.transform.SetParent(stageManager.AccessoryPool.transform);
        // Move it back beneath the stage
        accessorySlot.transform.localPosition = new Vector3(0, -5f, 0);
        accessorySlot.transform.localRotation = Quaternion.identity;
        accessorySlot.transform.localScale = Vector3.one;
    }

    /// <summary>
    /// Fetches all available accessory slots from the car (set in the inspector).
    /// </summary>
    void FetchAvailableAccessorySlots()
    {
        foreach (AccessorySlot accessorySlot in GetComponentsInChildren<AccessorySlot>())
        {
            AvailableAccessorySlots.Add(accessorySlot);
        }
    }

    /// <summary>
    /// Currently we only set the color of a car as a whole.
    /// TODO Expand to allow for individual car part colors to be selected using ComponentSelector handles as set up in the project.
    /// </summary>
    /// <param name="carColor"></param> the color to be set
    void SetSelectedCarPartColor(CarColorMaterial carColor)
    {
        SelectedCarPart.SetColor(carColor);
    }

    /// <summary>
    /// Helper functions to calculate a mathematical modulo that doesn't return
    /// negative values.
    /// </summary>
    float mod(float a, float b) { return ((a %= b) < 0) ? a + b : a; }
    int mod(int a, int b) { return ((a %= b) < 0) ? a + b : a; }
}
