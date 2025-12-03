using UnityEngine;

[CreateAssetMenu(fileName = "Part", menuName = "Scriptable Objects/Car/Parts/Combo Part", order = 1)]
public class CarComboPart : ScriptableObject
{
    public CarPart firstPart;
    public CarPart secondPart;
    public CarPart thirdPart;
}