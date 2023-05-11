using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Visualises the car's specs on the car selection screen.
/// </summary>
public class SpecsPanel : MonoBehaviour
{
    [SerializeField] Slider topSpeedSlider;
    [SerializeField] Slider accelerationSlider;
    [SerializeField] Slider handlingSlider;

    [SerializeField] float animationSpeed = 1f;

    public void SetSliderValues(int topSpeed, int acceleration, int handling)
    {
        StopAllCoroutines();
        StartCoroutine(AnimateSliderValueChange(topSpeedSlider, topSpeed));
        StartCoroutine(AnimateSliderValueChange(accelerationSlider, acceleration));
        StartCoroutine(AnimateSliderValueChange(handlingSlider, handling));
    }

    /// <summary>
    /// Animates the slider value change to make it more visually appealing as 
    /// the user cycles between different car models.
    /// </summary>
    /// <param name="slider"></param>
    /// <param name="targetValue"></param>
    /// <returns></returns>
    IEnumerator AnimateSliderValueChange(Slider slider, int targetValue)
    {
        float progress = 0f;
        float startValue = slider.value;

        while (Mathf.Abs(targetValue - slider.value) > 1)
        {
            progress += Time.deltaTime * animationSpeed;
            slider.value = Mathf.Lerp(startValue, targetValue, progress);
            yield return null;
        }
        slider.value = targetValue;
    }

}
