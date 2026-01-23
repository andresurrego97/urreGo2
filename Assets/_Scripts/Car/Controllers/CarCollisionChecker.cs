using System;
using UnityEngine;

public class CarCollisionChecker : MonoBehaviour
{
    public Action<float> OnColission;

    [SerializeField] private Rigidbody rb;

    private void OnCollisionEnter(Collision collision)
    {
        OnColission?.Invoke(rb.linearVelocity.magnitude);
    }
}