using UnityEngine;

[CreateAssetMenu(fileName = "Parts List", menuName = "Scriptable Objects/Car/List", order = 2)]
public class CarPartsList : ScriptableObject
{
    public CarDescription[] cars;
    public CarPart[] steeringWheels;
    public CarPart[] wheels;
    public CarComboPart[] comboWheels;
}