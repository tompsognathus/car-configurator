using UnityEngine;

/// <summary>
/// Used to set up colors/materials used for any of the cars or accessories
/// </summary>
[CreateAssetMenu(menuName = "CarConfigurator/CarColor")]
public class CarColorMaterial : ScriptableObject
{
    public string colorName;
    public Material material;
}