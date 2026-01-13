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

    private CarEngineStatus currentEngineStatus = CarEngineStatus.Idle;
    private float move;
    private float currentMove;
    private float currentMoveSteering;
    private bool accelerate;
    private float accelerating;
    private bool brake;
    private float braking;
    private bool handBrake;
    private bool inDirectionReverse;
    private bool inManualReverse;
    private bool turbo;

    private int hits = 0;
    private readonly RaycastHit[] hitNear = new RaycastHit[1];

    private float currentSpeed;
    private float rotate;

    [Space]
    private bool inDrift = false;
    private bool driftParticles = false;
    private float timeFullToDrift = 0;

    public void Move(InputAction.CallbackContext ctx)
    {
        move = ctx.ReadValue<float>();

        if (move != 1 || move != -1)
        {
            timeFullToDrift = 0;
        }
    }

    public void Accelerate(InputAction.CallbackContext ctx)
    {
        accelerating = ctx.ReadValue<float>();
        accelerate = ctx.performed;

        if (currentEngineStatus != CarEngineStatus.HandBreaking)
        {
            if (ctx.started && !brake)
            {
                currentEngineStatus = CarEngineStatus.Accelerating;
            }
            else if (ctx.canceled)
            {
                if (!brake)
                {
                    currentEngineStatus = CarEngineStatus.Idle;
                }
                else if (brake)
                {
                    currentEngineStatus = CarEngineStatus.Braking;
                }
            }
        }

        if (!accelerate && inDrift)
        {
            inDrift = false;
        }

        if (ctx.canceled)
        {
            timeFullToDrift = 0;
        }
    }

    public void Brake(InputAction.CallbackContext ctx)
    {
        braking = ctx.ReadValue<float>();
        brake = ctx.performed;

        if (currentEngineStatus != CarEngineStatus.HandBreaking)
        {
            if (ctx.started)
            {
                currentEngineStatus = CarEngineStatus.Braking;
            }
            else if (ctx.canceled)
            {
                if (ctx.canceled && !accelerate)
                {
                    currentEngineStatus = CarEngineStatus.Idle;
                }
                else if (ctx.canceled && accelerate)
                {
                    currentEngineStatus = CarEngineStatus.Accelerating;
                }
            }
        }
    }

    public void HandBrake(InputAction.CallbackContext ctx)
    {
        handBrake = ctx.performed;

        if (ctx.started)
        {
            currentEngineStatus = CarEngineStatus.HandBreaking;
        }
        else if (ctx.canceled)
        {
            if (brake)
            {
                currentEngineStatus = CarEngineStatus.Braking;
            }
            else if (accelerate)
            {
                currentEngineStatus = CarEngineStatus.Accelerating;
            }
            else
            {
                currentEngineStatus = CarEngineStatus.Idle;
            }
        }
    }

    public void Turbo(InputAction.CallbackContext ctx)
    {
        turbo = ctx.performed;
    }

    private void Update()
    {
        inDirectionReverse = Vector3.Dot(carParent.transform.forward, sphere.linearVelocity.normalized) > 0;
        inManualReverse = inDirectionReverse && !accelerate && !handBrake;
        velocity = sphere.linearVelocity.magnitude * (inManualReverse ? -1 : 1);

        hits = Physics.RaycastNonAlloc(carRoot.position + (carRoot.up * 0.1f), Vector3.down, hitNear, 2.0f);
        carNormal.up = Vector3.Lerp(carNormal.up, hitNear[0].normal, Time.deltaTime * 7.5f);



        switch (currentEngineStatus)
        {
            case CarEngineStatus.Idle:
                currentSpeed = 0;
                break;

            case CarEngineStatus.Accelerating:
                currentSpeed = speed * accelerating;
                break;

            case CarEngineStatus.Braking:
                //currentSpeed = speed * (accelerating - braking);
                currentSpeed = -reverse * (braking - accelerating);
                break;

            case CarEngineStatus.HandBreaking:
                currentSpeed = (!inDirectionReverse ? -reverse : reverse * 0.5f) * Mathf.Clamp01(velocity);
                break;
        }



        if (accelerate && (brake || handBrake))
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
        currentMove = Mathf.Lerp(currentMove, move * (inDrift ? 2 : 1), Time.deltaTime * (inDrift ? 0.6f : 8)); // variar intensidad por inDrift
        currentMoveSteering = Mathf.Lerp(currentMoveSteering, move, Time.deltaTime * 8);

        if (inDrift && move == 0 && currentMove < 0.25f && currentMove > -0.25f) // si deja de girar se cancela el drift, se asume que ya anda el carro derecho
        {
            inDrift = false;
            timeFullToDrift = 0;
        }

        //rotate = currentMove * rotation * Time.deltaTime /** customizer.currentCar.performance.steering.Evaluate(accelerate)*/ * Mathf.Clamp01(velocity * 0.1f);
        rotate = currentMove * rotation * Time.deltaTime * Mathf.Clamp(velocity * 0.1f, -1, 1);

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

        //customizer.currentRootReferences.root_steeringWheel

        if (!brake || (brake && inManualReverse))
        {
            for (int i = 0; i < customizer.currentRootReferences.root_frontWheels.Length; i++)
            {
                //TODO: Cambiar por el analogo de acelerar (tener en cuenta que botones llevaran a 1 de inmediato)
                customizer.currentRootReferences.root_frontWheels[i].Rotate(Vector3.forward, (i == 0 ? -velocity : velocity) * Time.deltaTime * 100); //hotfix de rotacion derecha inverso izquierda
            }
        }
        if (!handBrake)
        {
            for (int i = 0; i < customizer.currentRootReferences.root_backWheels.Length; i++)
            {
                //TODO: Cambiar por el analogo de acelerar (tener en cuenta que botones llevaran a 1 de inmediato)
                customizer.currentRootReferences.root_backWheels[i].Rotate(Vector3.forward, (i == 0 ? -velocity : velocity) * Time.deltaTime * 100); //hotfix de rotacion derecha inverso izquierda
            }
        }
        // cuano se tenga el "InReverse", poner las llantas reversando

        if (inDrift && !driftParticles)
        {
            driftParticles = true;

            for (int i = 0; i < customizer.currentRootReferences.particles_drift.Length; i++)
            {
                customizer.currentRootReferences.particles_drift[i].Play();
            }
        }
        else if (!inDrift && driftParticles)
        {
            driftParticles = false;

            for (int i = 0; i < customizer.currentRootReferences.particles_drift.Length; i++)
            {
                customizer.currentRootReferences.particles_drift[i].Stop(false, ParticleSystemStopBehavior.StopEmitting);
            }
        }
    }

    private void FixedUpdate()
    {
        if (currentEngineStatus != CarEngineStatus.Idle)
        {
            sphere.AddForce(-carParent.transform.forward * currentSpeed, ForceMode.Acceleration);
        }
    }
}