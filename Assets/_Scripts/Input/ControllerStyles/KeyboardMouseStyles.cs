using UnityEngine;

[CreateAssetMenu(fileName = "KeyboardMouse", menuName = "Scriptable Objects/Controller Styles/Keyboard Mouse", order = 3)]
public class KeyboardMouseStyles : ScriptableObject
{
    [Header("Key Blanks")]
    public Sprite enter;
    public Sprite superWide;
    public Sprite tall;
    public Sprite wide;
    public Sprite normal;

    [Header("Mouse")]
    public Sprite mouse;
    public Sprite mouseLeft;
    public Sprite mouseMiddle;
    public Sprite mouseRight;

    [Header("Ohters")]
    public Sprite diagram;
}