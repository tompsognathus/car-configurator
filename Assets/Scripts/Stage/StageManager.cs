using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The stage manager is responsible for controlling what happens on stage.
/// This includes rotating the stage, selecting cars, and selecting accessories.
/// </summary>
public class StageManager : MonoBehaviour
{
    [field: SerializeField] public AccessoryPool AccessoryPool { get; private set; }
    [SerializeField] float stageRotationSpeed = 10f;
    [SerializeField] Car[] cars;

    public int NumCars { get { return cars.Length; } }


    // Stage rotation variables
    float rotationDegrees = 90f;
    float currentStageAngle = 0f;
    bool isRotating = false;

    // Car selection variables
    int selectedCarIdx = 0;
    public Car SelectedCar { get { return cars[selectedCarIdx]; } }

    void Start()
    {
        DisableAllCars();
        SetUpInitialCar();
        EventManager.Instance.InvokeRequestUIUpdate();
    }

    // Getters
    public int GetSelectedCarPrice()
    {
        return SelectedCar.GetPriceWithSelectedAccessories();
    }
    public string GetSelectedCarBrandAndModel()
    {
        return SelectedCar.GetBrandAndModel();
    }

    public Car[] GetAllCars()
    {
        return cars;
    }

    // Car Management
    /// <summary>
    /// Prepares the stage to display the next car, moving it from the car pool
    /// to the backstage area and enabling it. Then, the stage can be rotated to
    /// present this new car to the user.
    /// 
    /// Called directly by UI buttons
    /// </summary>
    public void ShowNextCar()
    {
        if (isRotating) { return; }

        SetUpStageForRotation(NavDirection.Next);

        // Deselect current car
        cars[selectedCarIdx].OnSelectedOrUnselected(false);
        // Select next car
        selectedCarIdx = (mod((selectedCarIdx + 1), cars.Length));
        cars[selectedCarIdx].OnSelectedOrUnselected(true);

        RotatePlatform(NavDirection.Next);
        EventManager.Instance.InvokeRequestUIUpdate();
    }

    /// <summary>
    /// TODO consider merging with ShowNextCar, taking a parameter for navdirection. Not sure how to best handle the sign
    /// </summary>
    public void ShowPreviousCar()
    {
        if (isRotating) { return; }

        SetUpStageForRotation(NavDirection.Prev);

        // Deselect current car
        cars[selectedCarIdx].OnSelectedOrUnselected(false);
        // Select next car
        selectedCarIdx = (mod((selectedCarIdx - 1), cars.Length));
        cars[selectedCarIdx].OnSelectedOrUnselected(true);

        RotatePlatform(NavDirection.Prev);
        EventManager.Instance.InvokeRequestUIUpdate();
    }

    // Accessory Management

    /// <summary>
    /// Moves the next available accessory in the car's accessory list from
    /// the accessory pool to the car.
    /// </summary>
    public void ShowNextAccessory()
    {
        if (SelectedCar.AvailableAccessorySlots.Count > 0)
        {
            SelectedCar.ShowNextAccessorySlot();
        }
        else
        {
            Debug.LogError("No accessories available for this car");
            // This should never be executed as we skip the entire canvas
            // if there are no accessories available, but keeping it as a 
            // safeguard for if changes are made elsewhere
        }
    }
    /// <summary>
    /// Moves the previous available accessory in the car's accessory list from
    /// the accessory pool to the car. This should perhaps be removed and just left
    /// to the car class. Initially the idea was to only have the StageManager and
    /// UIManager communicating directly, but I've moved away from that after
    /// adding events.
    /// </summary>
    public void ShowPreviousAccessory()
    {
        if (SelectedCar.AvailableAccessorySlots.Count > 0)
        {
            SelectedCar.ShowPreviousAccessorySlot();
        }
        else
        {
            Debug.LogError("No accessories available for this car");
            // This should never be executed as we skip the entire canvas
            // if there are no accessories available, but keeping it as a 
            // safeguard for if changes are made elsewhere
        }
    }

    public int CountAvailableAccessories()
    {
        return SelectedCar.AvailableAccessorySlots.Count;
    }

    void DisableAllCars()
    {
        foreach (Car car in cars)
        {
            car.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Before a car gets displayed on stage, it is prepared backstage by setting
    /// the correct positioning and orientation, as well as enabling it.
    /// </summary>
    /// <param name="navDir"></param>
    void PrepareUpcomingCar(NavDirection navDir)
    {
        int directionMultiplier = navDir == NavDirection.Next ? 1 : -1;
        int upcomingCarIdx = mod(selectedCarIdx + directionMultiplier, cars.Length);
        float targetStageAngle = mod(currentStageAngle - directionMultiplier * rotationDegrees, 360);

        EnableCar(upcomingCarIdx);
        SetUpcomingCarRotation(navDir, upcomingCarIdx);

        // Set new car position
        SetUpcomingCarPosition(upcomingCarIdx, targetStageAngle);
    }

    /// <summary>
    /// A car that's no longer visible on stage is disabled and moved back to the car pool below the stage.
    /// </summary>
    /// <param name="navDir"></param>
    void HideOldCar(NavDirection navDir)
    {
        int directionMultiplier = navDir == NavDirection.Next ? 1 : -1;
        int oldCarIdx = mod(selectedCarIdx - directionMultiplier, cars.Length);

        DisableOldCar(oldCarIdx);
        ResetCarPosition(oldCarIdx);
    }

    void EnableCar(int carIdx)
    {
        cars[carIdx].gameObject.SetActive(true);
    }

    /// <summary>
    /// Rotates each car on stage to be presented at the desired angle when it
    /// appears on screen.
    /// </summary>
    /// <param name="navDir"></param> are we preparing the next or previous car in the list?
    /// <param name="upcomingCarIdx"></param> what's the index of the car we're preparing?
    void SetUpcomingCarRotation(NavDirection navDir, int upcomingCarIdx)
    {
        Quaternion upcomingCarRotation = Quaternion.identity;
        float upcomingCarRotationAdjustment = cars[upcomingCarIdx].CarPresentationAngle;

        if (navDir == NavDirection.Next)
        {
            upcomingCarRotation = Quaternion.Euler(0, 210f + upcomingCarRotationAdjustment, 0);
        }
        else
        {
            upcomingCarRotation = Quaternion.Euler(0, 390f + upcomingCarRotationAdjustment, 0);
        }
        cars[upcomingCarIdx].transform.rotation = upcomingCarRotation;
    }

    /// <summary>
    /// Positions a car on stage so that when the stage is rotated it will end up 
    /// on center stage
    /// </summary>
    /// <param name="upcomingCarIdx"></param> what's the index of the car we're preparing?
    /// <param name="targetStageAngle"></param> At what angle will the stage end up relative to its initial rotation
    void SetUpcomingCarPosition(int upcomingCarIdx, float targetStageAngle)
    {
        Vector3 newCarLocalPosition = Vector3.zero;
        float distanceFromCenterStage = cars[selectedCarIdx].DistanceFromCenterStage;

        if (targetStageAngle == 0)
        {
            newCarLocalPosition = new Vector3(0, cars[upcomingCarIdx].HeightAdjustment, -distanceFromCenterStage);
        }
        else if (targetStageAngle == 90f)
        {
            newCarLocalPosition = new Vector3(distanceFromCenterStage, cars[upcomingCarIdx].HeightAdjustment, 0);
        }
        else if (targetStageAngle == 180f)
        {
            newCarLocalPosition = new Vector3(0, cars[upcomingCarIdx].HeightAdjustment, distanceFromCenterStage);
        }
        else if (targetStageAngle == 270f)
        {
            newCarLocalPosition = new Vector3(-distanceFromCenterStage, cars[upcomingCarIdx].HeightAdjustment, 0);
        }
        else
        {
            Debug.LogError("Invalid currentStageAngle, should be in {0, 90, 180, 270}");
        }

        cars[upcomingCarIdx].transform.localPosition = newCarLocalPosition;
    }

    /// <summary>
    /// Moves car back to the car pool below the stage and resets its rotation
    /// </summary>
    /// <param name="carIdx"></param>
    void ResetCarPosition(int carIdx)
    {
        cars[carIdx].transform.localPosition = new Vector3(0, -5, 0);
        cars[carIdx].transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    private void DisableOldCar(int oldCarIdx)
    {
        cars[oldCarIdx].gameObject.SetActive(false);
    }

    /// <summary>
    /// We don't want to keep unnecessary cars active on stage, so we hid them
    /// once we're done with them.
    /// </summary>
    void SetUpStageForRotation(NavDirection navDir)
    {
        PrepareUpcomingCar(navDir);
        HideOldCar(navDir);
    }

    /// <summary>
    /// Slowly rotate the stage platform by 90 degrees in a given direction depending
    /// on whether we want to view the next or the previous car
    /// </summary>
    void RotatePlatform(NavDirection navDirection)
    {
        if (isRotating) { return; }
        isRotating = true;

        StartCoroutine(RotatePlatformCoroutine(navDirection));
    }

    // Coroutine to slowly rotate the platform by 90 degrees
    IEnumerator RotatePlatformCoroutine(NavDirection navDir)
    {
        float currentRotation = 0f;
        float rotationDirection = navDir == NavDirection.Next ? -1 : 1;

        while (currentRotation < rotationDegrees)
        {

            transform.Rotate(0, rotationDirection * stageRotationSpeed * Time.deltaTime, 0);
            currentRotation += stageRotationSpeed * Time.deltaTime;
            yield return null;
        }

        // Land on a precise angle at the end of the rotation
        currentStageAngle = mod((currentStageAngle + rotationDirection * rotationDegrees), 360);
        transform.rotation = Quaternion.Euler(0, currentStageAngle, 0);

        isRotating = false;
    }

    /// <summary>
    /// Set up the initial car on center stage
    /// </summary>
    void SetUpInitialCar()
    {
        cars[0].gameObject.SetActive(true);
        cars[0].OnSelectedOrUnselected(true);
        SetUpcomingCarPosition(0, 0);
        cars[0].transform.localRotation = Quaternion.Euler(0, 120 + cars[0].CarPresentationAngle, 0);
    }

    // Helper functions to compute a mathematical modulo that doesn't return negative values
    float mod(float a, float b) { return ((a %= b) < 0) ? a + b : a; }
    int mod(int a, int b) { return ((a %= b) < 0) ? a + b : a; }
}
