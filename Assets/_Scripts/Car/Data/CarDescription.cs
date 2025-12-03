using UnityEngine;

[CreateAssetMenu(fileName = "Car", menuName = "Scriptable Objects/Car/Car", order = 0)]
public class CarDescription : ScriptableObject
{
    public CarPart body;
    public CarPart bodyKit;
    public CarPart engine;
    public CarPart steeringWheel;
    public CarComboPart wheel;
}