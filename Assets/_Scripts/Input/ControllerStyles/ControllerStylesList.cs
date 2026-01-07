using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "ControllerStyles List", menuName = "Scriptable Objects/Controller Styles/List", order = -1)]
public class ControllerStylesList : ScriptableObject
{
    [Header("Keyboard & Mouse")]
    public KeyboardMouseStyles keyboardMouse_Style;

    [Header("Gamepads")]
    public GamepadStyles xBox_Style;
    public GamepadStyles ps5_Style;
    public GamepadStyles ps4_Style;
    public GamepadStyles ps3_Style;
    public GamepadStyles switch_Style;

    [Header("Joystick")]
    public JoystickStyles joystick_Style;

    [Header("Others")]
    public TextMeshProUGUI textPrefab;
    public Sprite controllerDisconnected;
}