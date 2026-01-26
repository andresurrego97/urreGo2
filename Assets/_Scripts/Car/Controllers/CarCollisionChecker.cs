using System;
using UnityEngine;

public class CarCollisionChecker : MonoBehaviour
{
    public Action<float> OnColission;

    private void OnCollisionEnter(Collision collision)
    {
        OnColission?.Invoke(collision.impulse.magnitude);
    }
}