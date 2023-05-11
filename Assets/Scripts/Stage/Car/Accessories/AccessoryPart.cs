using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Currently only used to store the default material of each individual
/// accessory component. We need this in order to be able to make an accessory
/// visible again after it was deselected and made semi-transparent.
/// </summary>
public class AccessoryPart : MonoBehaviour
{
    [field: SerializeField] public Material DefaultMaterial { get; private set; }       
}
