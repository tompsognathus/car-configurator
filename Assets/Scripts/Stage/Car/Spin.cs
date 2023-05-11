using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used to have cars slowly spin on the stage 
/// </summary>
public class Spin : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 10f;
    [SerializeField] bool rotateClockwise = true;
    [SerializeField] bool rotateOn = false;

    
    void Update()
    {
        RotateObject();    
    }

    private void RotateObject()
    {
        if (!rotateOn) { return; }
        
        if (rotateClockwise)
        {
            rotationSpeed = Mathf.Abs(rotationSpeed);
        } 
        else
        {
            rotationSpeed = -Mathf.Abs(rotationSpeed);
        }

        transform.Rotate(0, rotationSpeed*Time.deltaTime, 0);
    }
}
