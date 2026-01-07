using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;
using UnityEngine.InputSystem.Switch;
using UnityEngine.InputSystem.XInput;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ImageDeviceChanger : MonoBehaviour
{
    private enum SpriteType
    {
        ActionReference,
        Diagram
    }

    [SerializeField] private SpriteType spriteType;
    [SerializeField] private int controlIndex = 0;
    [SerializeField] private InputActionProperty actionReference;

    private InputAction myAction;
    private InputControl control = null;
    private string controlName;
    private string controlDisplayName;

    private InputSystemManager inputSystemManager;
    private Image image;
    private TextMeshProUGUI txtPrefab;

    private readonly Vector3 vector3Half = new(0.5f, 0.5f, 0.5f);
    private readonly Vector3 vector3Eighty = new(0.8f, 0.8f, 0.8f);

    private void Awake()
    {
        inputSystemManager = GetComponentInParent<InputSystemManager>();

        if (inputSystemManager == null)
            return;

        inputSystemManager.OnControlsChanged += ChangeSprite;

        image = GetComponent<Image>();
    }

    private void OnDestroy()
    {
        inputSystemManager.OnControlsChanged -= ChangeSprite;
    }

    private void Start()
    {
        ChangeSprite();
    }

    [ContextMenu("Manual change sprite")]
    public void ChangeSprite()
    {
        control = null;
        myAction = inputSystemManager.playerInput.actions.FindAction(actionReference.action.name);

        //for (int i = 0; i < inputSystemManager.playerInput.actions.actionMaps.Count; i++)
        //{
        //    for (int j = 0; j < inputSystemManager.playerInput.actions.actionMaps[i].actions.Count; j++)
        //    {
        //        if (inputSystemManager.playerInput.actions.actionMaps[i].actions[j].name == actionReference.action.name)
        //        {
        //            myAction = inputSystemManager.playerInput.actions.actionMaps[i].actions[j];
        //            break;
        //        }
        //    }
        //}

        if (myAction.controls.Count > controlIndex)
        {
            control = myAction.controls[controlIndex].device;
            controlName = myAction.controls[controlIndex].name;
            controlDisplayName = myAction.controls[controlIndex].displayName;
        }
        /// Extremely hacky
        //else if (inputSystemManager.playerInput.devices.Count > 0)
        //{
        //    control = inputSystemManager.playerInput.devices[0].device;
        //    controlName = OptimizePath(myAction.bindings[controlIndex].path);
        //    controlDisplayName = CapitalizeFirstLetter(controlName);
        //}

        if (control == null)
        {
            gameObject.SetActive(false);
            return;
        }

        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        switch (control)
        {
            case Mouse:
            case Keyboard:
                if (spriteType == SpriteType.ActionReference)
                    Set_KeyboardMouse();
                else
                    image.sprite = inputSystemManager.stylesList.keyboardMouse_Style.diagram;
                break;

            case DualSenseGamepadHID:
                if (spriteType == SpriteType.ActionReference)
                    Set_Gamepad(inputSystemManager.stylesList.ps5_Style);
                else
                    image.sprite = inputSystemManager.stylesList.ps5_Style.diagram;
                break;

            case DualShock4GamepadHID:
                if (spriteType == SpriteType.ActionReference)
                    Set_Gamepad(inputSystemManager.stylesList.ps4_Style);
                else
                    image.sprite = inputSystemManager.stylesList.ps4_Style.diagram;
                break;

            case DualShock3GamepadHID:
                if (spriteType == SpriteType.ActionReference)
                    Set_Gamepad(inputSystemManager.stylesList.ps3_Style);
                else
                    image.sprite = inputSystemManager.stylesList.ps3_Style.diagram;
                break;

            case DualShockGamepad:
                if (spriteType == SpriteType.ActionReference)
                    Set_Gamepad(inputSystemManager.stylesList.ps3_Style);
                else
                    image.sprite = inputSystemManager.stylesList.ps3_Style.diagram;
                break;

            case SwitchProControllerHID:
                if (spriteType == SpriteType.ActionReference)
                    Set_Gamepad(inputSystemManager.stylesList.switch_Style);
                else
                    image.sprite = inputSystemManager.stylesList.switch_Style.diagram;
                break;

            case XInputController:
                if (spriteType == SpriteType.ActionReference)
                    Set_Gamepad(inputSystemManager.stylesList.xBox_Style);
                else
                    image.sprite = inputSystemManager.stylesList.xBox_Style.diagram;
                break;

            case Gamepad:
                if (spriteType == SpriteType.ActionReference)
                    Set_Gamepad(inputSystemManager.stylesList.xBox_Style);
                else
                    image.sprite = inputSystemManager.stylesList.xBox_Style.diagram;
                break;

            case Joystick:
                if (spriteType == SpriteType.ActionReference)
                    Set_Joystick();
                else
                    image.sprite = inputSystemManager.stylesList.joystick_Style.diagram;
                break;

            default:
                //Set_Gamepad(inputSystemManager.stylesList.xBox_Style);
                break;
        }

        //Debug.LogWarning(inputActionProperty.action.actionMap.name); ///Action Map //UI
        //Debug.LogWarning(inputActionProperty.action.name); ///Action //Submit
        //Debug.LogWarning(inputActionProperty.action.type); ///Action Type //Button
        //Debug.LogWarning(myAction.bindings[0]); //Submit:<Joystick>/trigger[Touch;Joystick;XR]
        //Debug.LogWarning(myAction.bindings[0].name);
        //Debug.LogWarning(myAction.bindings[0].isComposite);
        //Debug.LogWarning(myAction.bindings[0].path); //<Joystick>/trigger
        //Debug.LogWarning(myAction.bindings[0].effectivePath);
        //Debug.LogWarning(myAction.bindings[0].overridePath);
        //Debug.LogWarning(myAction.bindings[0].action); //Submit
        //Debug.LogWarning(myAction.bindings[0].groups); ///Control scheme

        //Debug.LogWarning(myAction.controls[0].device.name); ///nombre local //USB,2-axis 8-button gamepad //Keyboard //XInputControllerWindows
        //Debug.LogWarning(myAction.controls[0].device.displayName); ///nombre en idioma mas legible //USB,2-axis 8-button gamepad //Keyboard //Xbox Controller
        //Debug.LogWarning(myAction.controls[0].usages[0]); /*No siempre sirve*/ ///Es como se llama esa dentro de la Action //PrimaryTrigger //submit //PrimaryAction
        //Debug.LogWarning(myAction.controls[0].name); ///Es la tecla especifica o boton especifico //trigger //enter //buttonSouth //button2
        //Debug.LogWarning(myAction.controls[0].displayName); ///tecla especifica en idioma mas legible //Trigger //Entrar //A //Button 2
    }

    //private string OptimizePath(string path)
    //{
    //    int lastSlash = path.LastIndexOf('/');
    //    ReadOnlySpan<char> span = lastSlash >= 0 ? path.AsSpan(lastSlash + 1).Trim('{') : path.AsSpan().Trim('{');

    //    return span.Trim('}').ToString();
    //}

    //private string CapitalizeFirstLetter(string text)
    //{
    //    Span<char> span = stackalloc char[text.Length];
    //    text.AsSpan().CopyTo(span);
    //    span[0] = char.ToUpper(span[0]);

    //    return new string(span);
    //}

    private void Set_KeyboardMouse()
    {
        if (controlName.Contains("Button"))
        {
            switch (controlName)
            {
                case "leftButton":
                    image.sprite = inputSystemManager.stylesList.keyboardMouse_Style.mouseLeft;
                    break;

                case "rightButton":
                    image.sprite = inputSystemManager.stylesList.keyboardMouse_Style.mouseRight;
                    break;

                case "middleButton":
                    image.sprite = inputSystemManager.stylesList.keyboardMouse_Style.mouseMiddle;
                    break;

                default:
                    image.sprite = inputSystemManager.stylesList.keyboardMouse_Style.mouse;
                    txtPrefab = Instantiate(inputSystemManager.stylesList.textPrefab, transform);
                    txtPrefab.SetText(controlDisplayName);
                    break;
            }
        }
        else
        {
            txtPrefab = Instantiate(inputSystemManager.stylesList.textPrefab, transform);

            switch (controlName)
            {
                case "enter":
                    image.sprite = inputSystemManager.stylesList.keyboardMouse_Style.enter;
                    txtPrefab.SetText(controlDisplayName);
                    break;

                case "space":
                    image.sprite = inputSystemManager.stylesList.keyboardMouse_Style.superWide;
                    txtPrefab.SetText(controlDisplayName);
                    break;

                case "numpadEnter":
                case "numpadPlus":
                case "backslash":
                    image.sprite = inputSystemManager.stylesList.keyboardMouse_Style.tall;
                    txtPrefab.SetText(controlDisplayName.Length > 7 ? controlDisplayName[..7] : controlDisplayName);
                    break;

                case "ctrl":
                case "leftCtrl":
                case "shift":
                case "leftShift":
                case "rightShift":
                case "capsLock":
                case "tab":
                case "backspace":
                case "numpad0":
                case "contextMenu":
                    image.sprite = inputSystemManager.stylesList.keyboardMouse_Style.wide;
                    txtPrefab.SetText(controlDisplayName.Length > 7 ? controlDisplayName[..7] : controlDisplayName);
                    break;

                case "leftArrow":
                    image.sprite = inputSystemManager.stylesList.keyboardMouse_Style.normal;
                    txtPrefab.SetText('\u2190'.ToString());
                    txtPrefab.transform.localScale = vector3Eighty;
                    break;
                case "upArrow":
                    image.sprite = inputSystemManager.stylesList.keyboardMouse_Style.normal;
                    txtPrefab.SetText('\u2191'.ToString());
                    txtPrefab.transform.localScale = vector3Eighty;
                    break;
                case "rightArrow":
                    image.sprite = inputSystemManager.stylesList.keyboardMouse_Style.normal;
                    txtPrefab.SetText('\u2192'.ToString());
                    txtPrefab.transform.localScale = vector3Eighty;
                    break;
                case "downArrow":
                    image.sprite = inputSystemManager.stylesList.keyboardMouse_Style.normal;
                    txtPrefab.SetText('\u2193'.ToString());
                    txtPrefab.transform.localScale = vector3Eighty;
                    break;

                default:
                    image.sprite = inputSystemManager.stylesList.keyboardMouse_Style.normal;
                    if (controlDisplayName.Length > 7)
                    {
                        txtPrefab.SetText(controlDisplayName[..7]);
                    }
                    else
                    {
                        txtPrefab.SetText(controlDisplayName);
                        txtPrefab.transform.localScale = vector3Eighty;
                    }
                    break;
            }
        }
    }

    private void Set_Gamepad(GamepadStyles style)
    {
        if (controlName == "dpad")
        {
            image.sprite = style.dpad;
        }
        else if (controlName == "leftStick")
        {
            image.sprite = style.leftStick;
        }
        else if (controlName == "rightStick")
        {
            image.sprite = style.rightStick;
        }
        else if (controlName == "stick")
        {
            image.enabled = false;
        }
        else
        {
            Span<SpriteBinding> spriteBindingsSpan = style.spriteBindings.AsSpan();

            for (int i = 0; i < spriteBindingsSpan.Length; i++)
            {
                if (spriteBindingsSpan[i].name == controlName)
                {
                    image.sprite = spriteBindingsSpan[i].sprite;
                    break;
                }
            }
        }
    }

    private void Set_Joystick()
    {
        if (controlName == "dpad" || controlName == "hat")
        {
            image.sprite = inputSystemManager.stylesList.joystick_Style.hat;
        }
        else if (controlDisplayName.Contains("Stick"))
        {
            InitializeStickControl("L");
        }
        else if (IsRzOrZControl())
        {
            InitializeStickControl("R");
        }
        else if (controlDisplayName.Contains("hat"))
        {
            switch (controlName)
            {
                case "up":
                    image.sprite = inputSystemManager.stylesList.joystick_Style.hat_up;
                    break;

                case "down":
                    image.sprite = inputSystemManager.stylesList.joystick_Style.hat_down;
                    break;

                case "left":
                    image.sprite = inputSystemManager.stylesList.joystick_Style.hat_left;
                    break;

                case "right":
                    image.sprite = inputSystemManager.stylesList.joystick_Style.hat_right;
                    break;
            }
        }
        else if (IsAxisOrVector2Control())
        {
            image.enabled = false;
        }
        else
        {
            txtPrefab = Instantiate(inputSystemManager.stylesList.textPrefab, transform);

            switch (controlName)
            {
                case "button1":
                case "trigger":
                    image.sprite = inputSystemManager.stylesList.joystick_Style.button1;
                    txtPrefab.SetText("1");
                    txtPrefab.transform.localScale = vector3Eighty;
                    break;

                case "button2":
                    image.sprite = inputSystemManager.stylesList.joystick_Style.button2;
                    txtPrefab.SetText("2");
                    txtPrefab.transform.localScale = vector3Eighty;
                    break;

                case "button3":
                    image.sprite = inputSystemManager.stylesList.joystick_Style.button3;
                    txtPrefab.SetText("3");
                    txtPrefab.transform.localScale = vector3Eighty;
                    break;

                case "button4":
                    image.sprite = inputSystemManager.stylesList.joystick_Style.button4;
                    txtPrefab.SetText("4");
                    txtPrefab.transform.localScale = vector3Eighty;
                    break;

                case "up":
                    image.sprite = inputSystemManager.stylesList.joystick_Style.button4;
                    txtPrefab.SetText("4");
                    txtPrefab.transform.localScale = vector3Eighty;
                    break;

                default:
                    image.sprite = inputSystemManager.stylesList.joystick_Style.trigger;
                    txtPrefab.SetText(controlName[^1].ToString());
                    txtPrefab.transform.localScale = vector3Eighty;
                    break;
            }
        }
    }

    private bool IsRzOrZControl()
    {
        return controlDisplayName == "Z" || controlDisplayName == "Rz" ||
               myAction.expectedControlType == "Vector2" && (controlDisplayName == "Z" || controlDisplayName == "Rz");
    }

    private bool IsAxisOrVector2Control()
    {
        return myAction.expectedControlType == "Axis" ||
               myAction.expectedControlType == "Vector2" && (controlDisplayName != "Z" || controlDisplayName != "Rz");
    }

    private void InitializeStickControl(string text)
    {
        image.sprite = inputSystemManager.stylesList.joystick_Style.stick;

        txtPrefab = Instantiate(inputSystemManager.stylesList.textPrefab, transform);
        txtPrefab.SetText(text);
        txtPrefab.transform.localScale = vector3Half;
    }
}