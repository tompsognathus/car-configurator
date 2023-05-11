using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    void Update()
    {
        // Quit game on ESC
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }        
    }
}
