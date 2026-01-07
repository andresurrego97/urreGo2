using UnityEngine;
using UnityEngine.InputSystem;

public class CarMovement : MonoBehaviour
{
    [SerializeField] CarDescription carDescription;

    [Space]
    [SerializeField] private Transform carRoot;
    [SerializeField] private Transform carNormal;
    [SerializeField] private Transform carParent;
    [SerializeField] private Transform carModel;
    [SerializeField] private Rigidbody sphere;

    [Space]
    [SerializeField] private Vector3 sphereOffset;
    [SerializeField] private float speed;
    [SerializeField] private float reverse;
    [SerializeField] private float rotation;

    [Space]
    public float velocity;

    private float move;
    private float currentMove;
    private bool accelerate; //TODO: Cambiar por el analogo de acelerar (tener en cuenta que botones llevaran a 1 de inmediato)
    private bool brake;
    private bool handBrake;
    private bool turbo;

    private int hits = 0;
    private readonly RaycastHit[] hitNear = new RaycastHit[1];

    private float currentSpeed;
    private float rotate;

    [Space]
    private bool inDrift = false;

    public void Move(InputAction.CallbackContext ctx)
    {
        move = ctx.ReadValue<float>();
    }

    //TODO: Cambiar por el analogo de acelerar (tener en cuenta que botones llevaran a 1 de inmediato)
    public void Accelerate(InputAction.CallbackContext ctx)
    {
        //TODO: Cambiar por el analogo de acelerar (tener en cuenta que botones llevaran a 1 de inmediato)
        accelerate = ctx.performed;

        if (!accelerate && inDrift)
        {
            inDrift = false;
        }
    }

    public void Brake(InputAction.CallbackContext ctx)
    {
        brake = ctx.performed;
    }

    public void HandBrake(InputAction.CallbackContext ctx)
    {
        handBrake = ctx.performed;
    }

    public void Turbo(InputAction.CallbackContext ctx)
    {
        turbo = ctx.performed;
    }

    private void Update()
    {
        hits = Physics.RaycastNonAlloc(carRoot.position + (carRoot.up * 0.1f), Vector3.down, hitNear, 2.0f);
        carNormal.up = Vector3.Lerp(carNormal.up, hitNear[0].normal, Time.deltaTime * 7.5f);

        if (brake)
        {
            currentSpeed = -reverse;
        }
        else if (accelerate)
        {
            currentSpeed = speed;
        }
        else
        {
            currentSpeed = 0;
        }

        if (accelerate && brake)
        {
            inDrift = true;
        }

        velocity = sphere.linearVelocity.magnitude;

        //currentMove = Mathf.Lerp(currentMove, move, Time.deltaTime * velocity.Remap(0, speed * 0.5f, 10, 1)); // variar intensidad de cambio de direccion dependiendo de la velocidad
        //currentMove = Mathf.Lerp(currentMove, move, Time.deltaTime * 10); // sin variacion de cambio de direccion por velocidad
        currentMove = Mathf.Lerp(currentMove, move, Time.deltaTime * (inDrift ? 1 : 8)); // variar intensidad por inDrift
        if (inDrift && move == 0 && currentMove < 0.2f && currentMove > -0.2f) // si deja de girar se cancela el drift, se asume que ya anda el carro derecho
        {
            inDrift = false;
        }

        //rotate = currentMove * rotation * Time.deltaTime /** carDescription.performance.steering.Evaluate(accelerate)*/ * Mathf.Clamp01(velocity * 0.1f);
        rotate = currentMove * rotation * Time.deltaTime * Mathf.Clamp01(velocity * 0.1f);

        carParent.localRotation = Quaternion.Euler(0, carParent.localEulerAngles.y + rotate, 0);

        carRoot.position = sphere.transform.position + sphereOffset;
    }

    private void FixedUpdate()
    {
        sphere.AddForce(-carModel.transform.forward * currentSpeed, ForceMode.Acceleration);
    }
}