using System;
using UnityEngine;

[Serializable]
public struct SpriteBinding
{
    public string name;
    public Sprite sprite;
}

[CreateAssetMenu(fileName = "Gamepad", menuName = "Scriptable Objects/Controller Styles/Gamepad", order = 0)]
public class GamepadStyles : ScriptableObject
{
    public SpriteBinding[] spriteBindings;

    [Header("Sticks")]
    public Sprite leftStick;
    public Sprite rightStick;

    [Header("Dpad")]
    public Sprite dpad;

    [Header("Ohters")]
    public Sprite diagram;
}