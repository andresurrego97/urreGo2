using System;
using UnityEngine;

[Serializable]
public struct CarColor
{
    public string name;
    public Color color;
}

[CreateAssetMenu(fileName = "Colors", menuName = "Scriptable Objects/Car/Colors", order = 1)]
public class CarColors : ScriptableObject
{
    public CarColor[] colors;
}