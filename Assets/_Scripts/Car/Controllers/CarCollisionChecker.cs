using System;
using UnityEngine;

public class CarCollisionChecker : MonoBehaviour
{
    public Action<float, Vector3> OnColission;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag(Tags.Road)) // hotfix no enviar info de colision a la camara cuando es la misma pista
            return;

        OnColission?.Invoke(collision.impulse.magnitude, collision.GetContact(0).point);
    }
}