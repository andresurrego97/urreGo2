using UnityEngine;

[CreateAssetMenu(fileName = "Joystick", menuName = "Scriptable Objects/Controller Styles/Joystick", order = 1)]
public class JoystickStyles : ScriptableObject
{
    [Header("Buttons")]
    public Sprite trigger;
    public Sprite button1;
    public Sprite button2;
    public Sprite button3;
    public Sprite button4;

    [Header("Hatswitch")]
    public Sprite hat;
    public Sprite hat_up;
    public Sprite hat_down;
    public Sprite hat_left;
    public Sprite hat_right;

    [Header("Stick")]
    public Sprite stick;

    [Header("Ohters")]
    public Sprite diagram;
}