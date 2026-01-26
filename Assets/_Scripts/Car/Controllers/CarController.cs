using UnityEngine;
using UnityEngine.InputSystem;

public class CarController : MonoBehaviour
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
    public float velocity;
    private Vector3 velocityVector;
    public float wheelsVelocity;

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

    [Space]
    private float currentSpeed;
    private float nextSpeed;
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
    private Quaternion currentRotation;
    private Quaternion targetYRotation;

    [Space]
    private float timeToExhaust;
    private float timeToExhaustRandom = 1;
    private float timeToExhaustDriftRandom = 1;

    [Space]
    private int detacheIndex;
    private Vector3 detachePower;

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

        if (ctx.started)
        {
            timeToExhaust = 1;
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

        if (ctx.started)
        {
            timeToExhaust = 1;
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

        if (ctx.started)
        {
            customizer.currentRootReferences.particles_exhaustMaterial.SetColor(CarColorsProperties.BaseColor, Color.cyan);
        }
        else if (ctx.canceled)
        {
            customizer.currentRootReferences.particles_exhaustMaterial.SetColor(CarColorsProperties.BaseColor, Color.white);
        }
    }

    private void Update()
    {
        if (customizer.currentRootReferences == null)
            return;

        inDirectionReverse = Vector3.Dot(carParent.forward, rb.linearVelocity.normalized) > 0;
        inManualReverse = inDirectionReverse && !accelerate && !handBrake;

        velocityVector = rb.linearVelocity;
        velocityVector.y = 0; // COMENTAR ESTO CUANDO HAYAN PISTAS CON RAMPAS, O CUANDO SE TENGAN PISTAS CON GRAVEDAD CUSTOM (Diferente a gravedad global hacia abajo)
        velocity = velocityVector.magnitude * (inManualReverse ? -1 : 1);


        if (isGrounded)
        {
            if (!handBrake && accelerate && velocity * 0.1f < accelerating)
            {
                wheelsDrift = true;
                wheelsVelocity = Mathf.Lerp(wheelsVelocity, customizer.currentCar.performance.acceleration * 0.25f * accelerating, Time.deltaTime * 2);
            }
            else
            {
                wheelsDrift = false;
                wheelsVelocity = velocity;
            }
        }
        else if (!handBrake && accelerate)
        {
            wheelsDrift = false;
            wheelsVelocity = Mathf.Lerp(wheelsVelocity, customizer.currentCar.performance.acceleration * 0.25f * accelerating, Time.deltaTime * 2);
        }
        else
        {
            wheelsDrift = false;
            wheelsVelocity = 0;
        }


        if (isGrounded)
        {
            switch (currentEngineStatus)
            {
                case CarEngineStatus.Idle:
                    nextSpeed = 0;
                    currentSpeed = Mathf.Lerp(currentSpeed, nextSpeed, Time.deltaTime);
                    break;

                case CarEngineStatus.Accelerating:
                    nextSpeed = turbo ? customizer.currentCar.performance.acceleration * 2 : customizer.currentCar.performance.acceleration * accelerating;
                    currentSpeed = Mathf.Lerp(currentSpeed, nextSpeed, Time.deltaTime * customizer.currentCar.performance.torque);
                    break;

                case CarEngineStatus.Braking:
                    nextSpeed = -(inManualReverse ? customizer.currentCar.performance.reverseAcceleration : customizer.currentCar.performance.reverseAcceleration * (inDrift ? 0.25f : 0.5f)) * (braking - accelerating);
                    currentSpeed = Mathf.Lerp(currentSpeed, nextSpeed, Time.deltaTime * (inManualReverse ? 15 : 5));
                    break;

                case CarEngineStatus.HandBreaking:
                    nextSpeed = (!inDirectionReverse ? -customizer.currentCar.performance.reverseAcceleration * 0.5f : customizer.currentCar.performance.reverseAcceleration * 0.25f) * Mathf.Clamp01(velocity);
                    currentSpeed = Mathf.Lerp(currentSpeed, nextSpeed, Time.deltaTime * 10);
                    break;
            }
        }
        else
        {
            currentSpeed = 0;
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

        moveCurve = move * customizer.currentCar.performance.steering.Evaluate(velocity / (customizer.currentCar.performance.acceleration * 0.5f));

        currentMove = Mathf.Lerp(currentMove, moveCurve * (inDrift ? 2 : 1), Time.deltaTime * currentMoveLerp); // variar intensidad por inDrift
        currentMoveSteering = Mathf.Lerp(currentMoveSteering, moveCurve, Time.deltaTime * 7.5f);

        if (inDrift)
        {
            if ((move == 0 && currentMove < 0.25f && currentMove > -0.25f && !handBrake) || // si deja de girar se cancela el drift, se asume que ya anda el carro derecho
                !isGrounded)
            {
                inDrift = false;
                timeFullToDrift = 0;
            }
        }

        rotate = currentMove * customizer.currentCar.performance.rotation * Time.deltaTime * Mathf.Clamp(velocity * 0.1f, -1, 1);

        carParent.localEulerAngles = new Vector3(0, carParent.localEulerAngles.y + rotate, 0);
        carNormal.rotation = Quaternion.LookRotation(carNormal.forward, rb.transform.up);
        carRoot.position = rb.transform.position;



        // Steering wheel rotation
        for (int i = 0; i < customizer.currentRootReferences.root_frontSteering.Length; i++)
        {
            customizer.currentRootReferences.root_frontSteering[i].localRotation =
                Quaternion.Euler(0, Mathf.Clamp(currentMoveSteering, -1, 1) * 45, 0);
        }


        // Car rotation
        customizer.currentRootReferences.root_steeringWheel.localRotation = Quaternion.Euler(0, 0, currentMoveSteering * 60);


        // Wheels rotation
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


        // Drift particles
        if ((!driftParticles && wheelsDrift) ||
            (!driftParticles && inDrift))
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


        // Exhaust particles
        if (accelerate)
        {
            timeToExhaust += Time.deltaTime * (wheelsDrift || turbo || handBrake ? customizer.currentCar.performance.torque * timeToExhaustRandom : customizer.currentCar.performance.torque * timeToExhaustDriftRandom) * accelerating;

            if (timeToExhaust >= 1)
            {
                timeToExhaust = 0;

                for (int i = 0; i < customizer.currentRootReferences.particles_exhaust.Length; i++)
                {
                    customizer.currentRootReferences.particles_exhaust[i].Play();
                }

                timeToExhaustRandom = turbo ? Random.Range(5f, 10f) : Random.Range(2f, 5f);
                timeToExhaustDriftRandom = inDrift ? Random.Range(1f, 2f) : Random.Range(0.2f, 0.4f);
            }
        }
        else
        {
            timeToExhaust = 0;
        }
    }

    private void FixedUpdate()
    {
        if (customizer.currentRootReferences == null)
            return;

        rb.angularDamping = isGrounded ? 10 : 0;
        //rb.maxAngularVelocity = 10;

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

        currentRotation = rb.transform.localRotation;
        targetYRotation = Quaternion.Euler(0f, carParent.localEulerAngles.y, 0f);
        rb.transform.localRotation =
            targetYRotation * Quaternion.Euler(currentRotation.eulerAngles.x, 0f, currentRotation.eulerAngles.z);


        //if (isGrounded && currentEngineStatus != CarEngineStatus.Idle)
        {
            rb.AddForce(-carParent.forward * currentSpeed, ForceMode.Acceleration);
        }
    }

    [ContextMenu("Detache part")]
    private void DetachePart()
    {
        if (customizer.currentRootReferences.removableParts.Count == 0)
            return;

        detacheIndex = Random.Range(0, customizer.currentRootReferences.removableParts.Count);

        customizer.currentRootReferences.removableParts[detacheIndex].collider.transform.SetParent(null);

        detachePower = customizer.currentRootReferences.removableParts[detacheIndex].rigidbody.position - rb.position;
        detachePower *= 2.5f;
        detachePower.y *= 2;

        customizer.currentRootReferences.removableParts[detacheIndex].rigidbody.isKinematic = false;
        customizer.currentRootReferences.removableParts[detacheIndex].rigidbody.AddForceAtPosition(
            detachePower,
            rb.position,
            ForceMode.Impulse);
        customizer.currentRootReferences.removableParts[detacheIndex].collider.enabled = true;

        customizer.currentRootReferences.removableParts.RemoveAt(detacheIndex);

        // Pasar logica a un script por componente para que luego pueda en la escala hacerse pequeño despues
        // de un tiempo y se destruya, para ahorrar carga
    }
}