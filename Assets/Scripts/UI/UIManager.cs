using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the different UI canvases and handles navigation between them.
/// </summary>
public class UIManager : MonoBehaviour
{
    [field: SerializeField] public CarColorMaterial DeselectedAccessoryColor { get; private set; }
    [SerializeField] StageManager stageManager;
    [SerializeField] List<Canvas> canvasList = new List<Canvas>();
    [SerializeField] public string CurrencySymbol { get; private set; } = "£";
    
    public int currentCanvasIdx = 0;

    void Start()
    {
        // Some canvases trigger events OnEnable, so we need to make sure they
        // are all disabled initially
        ActivateFirstUICanvas();
    }

    public void DisableAllUICanvases()
    {
        foreach (Canvas canvas in canvasList)
        {
            canvas.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Called when UI navigation buttons are clicked (e.g. Select or Back).
    /// Set up for each button in the inspector with OnClick()
    /// 
    /// Deactivates the canvas that's currently active and replaces it with the
    /// next canvas in the list.
    /// </summary>
    public void ShowNextUICanvas()
    {
        int nextCanvasIdx = (currentCanvasIdx + 1) % canvasList.Count;
        // If we're about to display the accessory selector and there aren't any accessories available, skip it
        bool isNextCanvasAccessorySelector = canvasList[nextCanvasIdx].GetComponentInChildren<AccessorySelectorCanvasManager>() != null;
        if (isNextCanvasAccessorySelector && stageManager.CountAvailableAccessories() == 0)
        {
            nextCanvasIdx = (nextCanvasIdx + 1) % canvasList.Count;
        }

        ToggleCanvasVisibility(nextCanvasIdx);
        EventManager.Instance.InvokeUICanvasChanged();
    }

    /// <summary>
    /// Called when UI navigation buttons are clicked (e.g. Select or Back).
    /// Set up for each button in the inspector with OnClick()
    /// 
    /// Deactivates the canvas that's currently active and replaces it with the
    /// previous canvas in the list.
    /// </summary>
    public void ShowPreviousUICanvas()
    {
        int previousCanvasIdx = currentCanvasIdx - 1 % canvasList.Count;
        
        // If we're about to display the accessory selector and there aren't any accessories available, skip it
        bool isPreviousCanvasAccessorySelector = canvasList[previousCanvasIdx].GetComponentInChildren<AccessorySelectorCanvasManager>() != null;
        if (isPreviousCanvasAccessorySelector && stageManager.CountAvailableAccessories() == 0)
        {
            previousCanvasIdx = (previousCanvasIdx - 1) % canvasList.Count;
        }
        
        ToggleCanvasVisibility(previousCanvasIdx);
        EventManager.Instance.InvokeUICanvasChanged();
    }

    /// <summary>
    /// Disables the current canvas and activates the next one
    /// </summary>
    void ToggleCanvasVisibility(int upcomingCanvasIdx)
    {
        canvasList[currentCanvasIdx].gameObject.SetActive(false);
        currentCanvasIdx = upcomingCanvasIdx;

        canvasList[upcomingCanvasIdx].gameObject.SetActive(true);
    }

    void ActivateFirstUICanvas()
    {
        foreach (Canvas canvas in canvasList)
        {
            canvas.gameObject.SetActive(false);
        }
        canvasList[currentCanvasIdx].gameObject.SetActive(true);
    }
}
