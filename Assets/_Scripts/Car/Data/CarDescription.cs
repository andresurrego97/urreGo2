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

[Serializable]
public struct CarPerformance
{
    [Tooltip("Acceleration and Top Speed")]
    public float acceleration;
    [Tooltip("Force or velocity at which it reaches that maximum speed")]
    public float torque;
    [Tooltip("Reverse speed and brake power")]
    public float reverseAcceleration;
    [Tooltip("How much can it rotate")]
    public float rotation;
    [Tooltip("Reduced steering response at high speed to maintain stability")]
    public AnimationCurve steering;

    [Header("Suspension")]
    [Tooltip("Distance from the suspension point to the ground. This must not be modified!")]
    public float suspensionLength;
    [Tooltip("Force exerted by each tire when the suspension is compressed, the higher it is, the faster it straightens")]
    public float suspensionForce;
    [Tooltip("The faster the suspension reaches zero to stabilize, but make it laggyer")]
    public float suspensionDamper;
}

[CreateAssetMenu(fileName = "Car", menuName = "Scriptable Objects/Car/Car", order = 0)]
public class CarDescription : ScriptableObject
{
    public CarPerformance performance;

    [Header("Parts")]
    public CarPart body;
    public CarPart bodyKit;
    public CarPart engine;
    public CarPart steeringWheel;
    public CarComboPart wheel;

    [Space]
    public CarDecal[] decals;
}