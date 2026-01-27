using Unity.Cinemachine;
using UnityEngine;

public class CarCamera : MonoBehaviour
{
    [SerializeField] private CarCustomizer customizer;
    [SerializeField] private CarController carMovement;
    [SerializeField] private CarCollisionChecker collisionChecker;
    [SerializeField] private CinemachineBasicMultiChannelPerlin cinemachineNoise;

    [Space]
    [SerializeField] private Transform cameraRoot;

    [Space]
    [SerializeField] private float frequencyMax;
    [SerializeField] private float frequencyAdd;

    [Space]
    [SerializeField] private float amplitudeBase;
    [SerializeField] private float shakeLimit;
    [SerializeField] private float shakePower;
    [SerializeField] private float shakeSpeed;

    private void Awake()
    {
        customizer.OnBodyChange += ChangeCameraRoot;
        collisionChecker.OnColission += CollisionShake;
    }

    private void OnDestroy()
    {
        customizer.OnBodyChange -= ChangeCameraRoot;
        collisionChecker.OnColission -= CollisionShake;
    }

    private void ChangeCameraRoot()
    {
        cameraRoot.SetLocalPositionAndRotation(customizer.currentRootReferences.cameraRoot.localPosition, customizer.currentRootReferences.cameraRoot.localRotation);
    }

    private void CollisionShake(float power, Vector3 _)
    {
        if (power > shakeLimit)
        {
            cinemachineNoise.AmplitudeGain += power * shakePower;
        }
    }

    private void Update()
    {
        if (customizer.currentRootReferences == null)
            return;

        cinemachineNoise.FrequencyGain =
            Mathf.Clamp((carMovement.wheelsVelocity / (customizer.currentCar.performance.acceleration * 0.4f)).Remap(0, 1, 0, frequencyMax), 0, frequencyMax * 2) + frequencyAdd;

        cinemachineNoise.AmplitudeGain = Mathf.Lerp(cinemachineNoise.AmplitudeGain, amplitudeBase, Time.deltaTime * shakeSpeed);
    }
}