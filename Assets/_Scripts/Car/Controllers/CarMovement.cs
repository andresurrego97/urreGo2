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
    [SerializeField] private Rigidbody rb;

    [Space]
    private float velocity;
    private Vector3 velocityVector;
    private float wheelsVelocity;

    [Space]
    private CarEngineStatus currentEngineStatus = CarEngineStatus.Idle;
    private float move;
    private float moveCurve;
    private float currentMove;
    private float currentMoveLerp;
    private float currentMoveSteering;
    private bool accelerate;
    private float accelerating;
    private bool brake;
    private float braking;
    private bool handBrake;
    private bool inDirectionReverse;
    private bool inManualReverse;
    private bool turbo;

    private float currentSpeed;
    private float rotate;

    [Space]
    private bool inDrift = false;
    private bool wheelsDrift = false;
    private bool driftParticles = false;
    private float timeFullToDrift = 0;

    [Space]
    private int suspensionIndex = 0;
    private RaycastHit hit;
    private Vector3[] force;
    private float suspensionDifference;
    private float suspensionWheelTravel;
    private float damp;
    private float z;
    private float multipler;
    private bool isGrounded;
    //private float chasisMove;
    //public float chasisMovePower;

    private void Awake()
    {
        // Setear esto al principio de una pista, para que los carros con 0 o mas de 4 llantas, no gasten cuando no deben
        suspensionIndex = CarSuspensionRaycasts.Instance.Reserve(4);
        force = new Vector3[4];
    }

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

        if (customizer.currentRootReferences == null)
            return;

        if (customizer.currentRootReferences.material_emissive != null)
        {
            if (ctx.started)
            {
                customizer.currentRootReferences.material_emissive.SetFloat(CarColorsProperties.EmissiveBoost, customizer.currentRootReferences.default_emissiveValue * 2);
            }
            else if (ctx.canceled)
            {
                customizer.currentRootReferences.material_emissive.SetFloat(CarColorsProperties.EmissiveBoost, customizer.currentRootReferences.default_emissiveValue);
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
        if (customizer.currentRootReferences == null)
            return;

        inDirectionReverse = Vector3.Dot(carParent.forward, rb.linearVelocity.normalized) > 0;
        inManualReverse = inDirectionReverse && !accelerate && !handBrake;

        velocityVector = rb.linearVelocity;
        velocityVector.y = 0;
        velocity = velocityVector.magnitude * (inManualReverse ? -1 : 1);



        if (!handBrake && accelerate && velocity * 0.1f < accelerating)
        {
            wheelsDrift = true;
            wheelsVelocity = Mathf.Lerp(wheelsVelocity, customizer.currentCar.performance.acceleration * 0.25f, Time.deltaTime * 2);
        }
        else
        {
            wheelsDrift = false;
            wheelsVelocity = velocity;
        }



        switch (currentEngineStatus)
        {
            case CarEngineStatus.Idle:
                currentSpeed = Mathf.Lerp(currentSpeed, 0, Time.deltaTime);
                break;

            case CarEngineStatus.Accelerating:
                currentSpeed = customizer.currentCar.performance.acceleration * accelerating;
                break;

            case CarEngineStatus.Braking:
                currentSpeed = -customizer.currentCar.performance.reverseAcceleration * (braking - accelerating);
                break;

            case CarEngineStatus.HandBreaking:
                currentSpeed = (!inDirectionReverse ? -customizer.currentCar.performance.reverseAcceleration : customizer.currentCar.performance.reverseAcceleration * 0.5f) * Mathf.Clamp01(velocity);
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

        if (inDrift)
        {
            if (!brake)
            {
                currentMoveLerp = 0.6f;
            }
            else
            {
                currentMoveLerp = 2;
            }
        }
        else
        {
            currentMoveLerp = 8;
        }
        //currentMoveLerp = inDrift && !brake ? 0.6f : 8;

        moveCurve = move * customizer.currentCar.performance.steering.Evaluate(velocity / (customizer.currentCar.performance.acceleration * 0.5f));

        currentMove = Mathf.Lerp(currentMove, moveCurve * (inDrift ? 2 : 1), Time.deltaTime * currentMoveLerp); // variar intensidad por inDrift
        currentMoveSteering = Mathf.Lerp(currentMoveSteering, moveCurve, Time.deltaTime * 7.5f);

        if (inDrift && move == 0 && currentMove < 0.25f && currentMove > -0.25f) // si deja de girar se cancela el drift, se asume que ya anda el carro derecho
        {
            inDrift = false;
            timeFullToDrift = 0;
        }

        //rotate = currentMove * rotation * Time.deltaTime /** customizer.currentCar.performance.steering.Evaluate(accelerate)*/ * Mathf.Clamp01(velocity * 0.1f);
        rotate = currentMove * customizer.currentCar.performance.rotation * Time.deltaTime * Mathf.Clamp(velocity * 0.1f, -1, 1);

        carParent.localRotation = Quaternion.Euler(0, carParent.localEulerAngles.y + rotate, 0);

        carNormal.up = rb.transform.up;
        carRoot.position = rb.transform.position;



        // Animations

        for (int i = 0; i < customizer.currentRootReferences.root_frontSteering.Length; i++)
        {
            customizer.currentRootReferences.root_frontSteering[i].localRotation =
                Quaternion.Euler(0, Mathf.Clamp(currentMoveSteering, -1, 1) * 45, 0);
        }

        customizer.currentRootReferences.root_steeringWheel.localRotation = Quaternion.Euler(0, 0, currentMoveSteering * 60);

        if (!brake || (brake && inManualReverse))
        {
            for (int i = 0; i < customizer.currentRootReferences.root_frontWheels.Length; i++)
            {
                customizer.currentRootReferences.root_frontWheels[i].Rotate(Vector3.forward, (i == 0 ? -velocity : velocity) * Time.deltaTime * 100); //hotfix de rotacion derecha inverso izquierda
            }
        }
        if (!handBrake)
        {
            for (int i = 0; i < customizer.currentRootReferences.root_backWheels.Length && i < 2; i++) //hotfix de solo 2 ruedas traseras maximo (baja truck fix)
            {
                customizer.currentRootReferences.root_backWheels[i].Rotate(Vector3.forward, (i == 0 ? -wheelsVelocity : wheelsVelocity) * Time.deltaTime * 100); //hotfix de rotacion derecha inverso izquierda
            }
        }

        if ((!driftParticles && wheelsDrift) ||
            (!driftParticles && inDrift) /*||
            (!driftParticles && accelerate && brake)*/)
        {
            driftParticles = true;

            for (int i = 0; i < customizer.currentRootReferences.particles_drift.Length; i++)
            {
                customizer.currentRootReferences.particles_drift[i].Play();
            }
        }
        else if (driftParticles && !inDrift && !wheelsDrift)
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
        if (customizer.currentRootReferences == null)
            return;

        rb.maxLinearVelocity = inManualReverse ? 6 : customizer.currentCar.performance.acceleration * 2;

        if (customizer.currentRootReferences.root_suspension.Length == 0)
        {
            z = rb.transform.localEulerAngles.z;

            if (z > 180f)
            {
                multipler = z.Remap(360, 180, 0, customizer.currentCar.performance.suspensionForce);
                z -= 360f;
            }
            else
            {
                multipler = z.Remap(0, 180, 0, customizer.currentCar.performance.suspensionForce);
            }

            rb.AddTorque(-z * multipler * rb.transform.forward, ForceMode.Acceleration);

            CarSuspensionRaycasts.Instance.SetCommand(
                suspensionIndex,
                customizer.currentRootReferences.transform.position + (customizer.currentRootReferences.transform.up.normalized * 0.5f),
                -customizer.currentRootReferences.transform.up,
                customizer.currentCar.performance.suspensionLength);

            isGrounded = CarSuspensionRaycasts.Instance.GetHit(suspensionIndex).collider;
        }
        else
        {
            isGrounded = false;

            //chasisMove = Mathf.Lerp(chasisMove, accelerating - braking, Time.deltaTime * 10);

            for (int i = 0; i < customizer.currentRootReferences.root_suspension.Length; i++)
            {
                CarSuspensionRaycasts.Instance.SetCommand(
                    suspensionIndex + i,
                    customizer.currentRootReferences.root_suspension[i].position,
                    -customizer.currentRootReferences.root_suspension[i].up,
                    customizer.currentCar.performance.suspensionLength);

                hit = CarSuspensionRaycasts.Instance.GetHit(suspensionIndex + i);

                if (!hit.collider)
                {
                    suspensionDifference = 0;
                    suspensionWheelTravel = -customizer.currentCar.performance.suspensionLength;
                }
                else
                {
                    isGrounded = true;

                    suspensionDifference = (customizer.currentCar.performance.suspensionLength - hit.distance) * customizer.currentCar.performance.suspensionForce;
                    suspensionWheelTravel = customizer.currentCar.performance.suspensionLength - hit.distance;

                    force[i] = suspensionDifference * customizer.currentRootReferences.root_suspension[i].up;
                    damp = (1f - Mathf.Exp(-force[i].magnitude * customizer.currentCar.performance.suspensionDamper * Time.deltaTime)).Remap(0, 1, 1, 0);
                    force[i] = Vector3.Lerp(force[i], Vector3.zero, damp);
                    //Debug.LogWarning($"hit #{i} Diference: {force[i]} Distance: {hit.distance} Resta: {customizer.currentCar.performance.suspensionLength - hit.distance}");

                    rb.AddForceAtPosition(
                        force[i],
                        customizer.currentRootReferences.root_suspension[i].position,
                        ForceMode.Force);
                }

                customizer.currentRootReferences.root_suspension[i].GetChild(0).localPosition = new Vector3(
                    customizer.currentRootReferences.root_suspension[i].GetChild(0).localPosition.x,
                    suspensionWheelTravel,
                    customizer.currentRootReferences.root_suspension[i].GetChild(0).localPosition.z);
            }
        }

        //

        rb.transform.localEulerAngles = new Vector3(rb.transform.localEulerAngles.x, carParent.localEulerAngles.y, rb.transform.localEulerAngles.z);

        //if (isGrounded && currentEngineStatus != CarEngineStatus.Idle)
        {
            rb.AddForce(-carParent.forward * currentSpeed, ForceMode.Acceleration);
        }
    }
}