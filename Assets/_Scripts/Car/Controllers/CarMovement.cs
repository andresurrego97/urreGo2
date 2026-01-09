using UnityEngine;
using UnityEngine.InputSystem;

public class CarMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] CarCustomizer customizer;

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
    private float currentMoveSteering;
    private bool accelerate; //TODO: Cambiar por el analogo de acelerar (tener en cuenta que botones llevaran a 1 de inmediato)
    private bool brake;
    private bool handBrake;
    private bool isGoinToStop;
    private bool inReverse;
    private bool turbo;

    private int hits = 0;
    private readonly RaycastHit[] hitNear = new RaycastHit[1];

    private float currentSpeed;
    private float rotate;

    [Space]
    private bool inDrift = false;
    private float timeFullToDrift = 0;

    public void Move(InputAction.CallbackContext ctx)
    {
        move = ctx.ReadValue<float>();

        if (move != 1 || move != -1)
        {
            timeFullToDrift = 0;
        }
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

        if (ctx.canceled)
        {
            timeFullToDrift = 0;
            inReverse = false;
        }
    }

    public void Brake(InputAction.CallbackContext ctx)
    {
        brake = ctx.performed;

        if (brake)
        {
            isGoinToStop = true;
        }
        if (ctx.canceled)
        {
            isGoinToStop = false;
            inReverse = false;
        }
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
        velocity = sphere.linearVelocity.magnitude;

        hits = Physics.RaycastNonAlloc(carRoot.position + (carRoot.up * 0.1f), Vector3.down, hitNear, 2.0f);
        carNormal.up = Vector3.Lerp(carNormal.up, hitNear[0].normal, Time.deltaTime * 7.5f);

        if (isGoinToStop && velocity <= 1)
        {
            inReverse = true;
        }

        if (accelerate && brake && velocity > 1 && !inReverse) // corregir que girar lento puede ocasionar que reverse sin ponerse en InReverse
        {
            currentSpeed = -reverse;
            inReverse = false;
            Debug.LogWarning("De acelerando a frenando al tiempo");
        }
        else if (accelerate && brake && inReverse)
        {
            currentSpeed = 0;
            Debug.LogWarning("acelerando y frenando al tiempo - Estatatico");
        }
        else if (accelerate)
        {
            currentSpeed = speed;
            Debug.LogWarning("Acelerando");
        }
        else if (brake)
        {
            currentSpeed = -reverse;
            Debug.LogWarning("Reversa");
        }
        else
        {
            currentSpeed = 0;
            Debug.LogWarning("Nada");
        }

        if (accelerate && brake)
        {
            inDrift = true;
        }
        if (!inDrift && accelerate && (move == 1 || move == -1) && timeFullToDrift < 1.5f)
        {
            timeFullToDrift += Time.deltaTime;

            if (timeFullToDrift >= 1.5f)
            {
                inDrift = true;
            }
        }

        //currentMove = Mathf.Lerp(currentMove, move, Time.deltaTime * velocity.Remap(0, speed * 0.5f, 10, 1)); // variar intensidad de cambio de direccion dependiendo de la velocidad
        //currentMove = Mathf.Lerp(currentMove, move, Time.deltaTime * 10); // sin variacion de cambio de direccion por velocidad
        currentMove = Mathf.Lerp(currentMove, move * (inDrift ? 2 : 1), Time.deltaTime * (inDrift ? 0.7f : 8)); // variar intensidad por inDrift
        currentMoveSteering = Mathf.Lerp(currentMoveSteering, move, Time.deltaTime * 8);

        if (inDrift && move == 0 && currentMove < 0.25f && currentMove > -0.25f) // si deja de girar se cancela el drift, se asume que ya anda el carro derecho
        {
            inDrift = false;
            timeFullToDrift = 0;
        }

        //rotate = currentMove * rotation * Time.deltaTime /** customizer.currentCar.performance.steering.Evaluate(accelerate)*/ * Mathf.Clamp01(velocity * 0.1f);
        rotate = currentMove * rotation * Time.deltaTime * Mathf.Clamp01(velocity * 0.1f);

        // Agregar estado de "InReverse" para que se invierta la rotacion cuando se esta reversando
        carParent.localRotation = Quaternion.Euler(0, carParent.localEulerAngles.y + rotate, 0);

        carRoot.position = sphere.transform.position + sphereOffset;


        // Animations

        if (customizer.currentRootReferences == null)
            return;

        for (int i = 0; i < customizer.currentRootReferences.root_frontSteering.Length; i++)
        {
            customizer.currentRootReferences.root_frontSteering[i].localRotation =
                Quaternion.Euler(0, Mathf.Clamp(currentMoveSteering, -1, 1) * 45 + customizer.currentRootReferences.frontSteeringOffset, 0);
        }

        if (!brake)
        {
            for (int i = 0; i < customizer.currentRootReferences.root_frontWheels.Length; i++)
            {
                //TODO: Cambiar por el analogo de acelerar (tener en cuenta que botones llevaran a 1 de inmediato)
                customizer.currentRootReferences.root_frontWheels[i].Rotate(Vector3.forward, (i == 0 ? -velocity : velocity) * Time.deltaTime * 100); //hotfix de rotacion derecha inverso izquierda
            }
        }
        if (!handBrake)
        {
            for (int i = 0; i < customizer.currentRootReferences.root_frontWheels.Length; i++)
            {
                //TODO: Cambiar por el analogo de acelerar (tener en cuenta que botones llevaran a 1 de inmediato)
                customizer.currentRootReferences.root_backWheels[i].Rotate(Vector3.forward, (i == 0 ? -velocity : velocity) * Time.deltaTime * 100); //hotfix de rotacion derecha inverso izquierda
            }
        }
        // cuano se tenga el "InReverse", poner las llantas reversando
    }

    private void FixedUpdate()
    {
        sphere.AddForce(-carParent.transform.forward * currentSpeed, ForceMode.Acceleration);
    }
}