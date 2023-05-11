using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Currently unused in practice except in experimental component selectors for
/// car parts. Can be attached to a component selector button to make sure it
/// always faces the camera.
/// </summary>
public class FaceCamera : MonoBehaviour
{
    [SerializeField] Camera mainCamera;

    void Update()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);
    }
}
