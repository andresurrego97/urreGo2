using System;
using UnityEngine;

[Serializable]
public struct CarDecal
{
    public string name; // default = "none"
    public Texture body;
    public Texture bodyKit;
    public Texture chasis;
    public Texture engine;
    public Texture dash;
    public Texture emissive;
    public Texture glass;
    public Texture interior;
}

[CreateAssetMenu(fileName = "Car", menuName = "Scriptable Objects/Car/Car", order = 0)]
public class CarDescription : ScriptableObject
{
    [Header("Parts")]
    public CarPart body;
    public CarPart bodyKit;
    public CarPart engine;
    public CarPart steeringWheel;
    public CarComboPart wheel;

    [Header("Decals")]
    //public CarDecal decalDefault;
    public CarDecal[] decals;
}