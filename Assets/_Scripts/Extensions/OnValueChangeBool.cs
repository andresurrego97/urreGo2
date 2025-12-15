using UnityEngine;
using UnityEngine.Events;

public class OnValueChangeBool : MonoBehaviour
{
    [SerializeField] private UnityEvent whenOn;
    [SerializeField] private UnityEvent whenOff;

    public void OnValueChange(bool value)
    {
        if (value)
        {
            whenOn?.Invoke();
        }
        else
        {
            whenOff?.Invoke();
        }
    }
}