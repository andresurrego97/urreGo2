
using Unity.Cinemachine;
using UnityEngine;

public class CarCamera : MonoBehaviour
{
    [SerializeField] private CarCustomizer customizer;
    [SerializeField] private CarMovement carMovement;
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
    [SerializeField] private float shakePowerLimit;
    private float shakeTime;
    private float shakeTimer;
    private float shakePowered;

    private void Awake()
    {
        customizer.OnBodyChange += ChangeCameraRoot;
        collisionChecker.OnColission += CollisionShake;
    }

    private void ChangeCameraRoot()
    {
        cameraRoot.SetLocalPositionAndRotation(customizer.currentRootReferences.cameraRoot.localPosition, customizer.currentRootReferences.cameraRoot.localRotation);
    }

    private void CollisionShake(float power)
    {
        Debug.LogWarning("__________________");
        Debug.LogWarning($"power: {power}");

        if (power >= shakeLimit)
        {
            shakeTime = Mathf.Clamp01(1 + power);
            Debug.LogWarning($"shakeTime: {shakeTime}");
            shakeTimer = 0;
            shakePowered = Mathf.Clamp(shakePower * power, 0, shakePowerLimit);
            Debug.LogWarning($"shakePowered: {shakePowered}");

            Debug.LogWarning($"cinemachineNoise.AmplitudeGain + shakePowered + amplitudeBase: {cinemachineNoise.AmplitudeGain + shakePowered + amplitudeBase}");
        }
    }

    private void Update()
    {
        if (customizer.currentRootReferences == null)
            return;

        cinemachineNoise.FrequencyGain =
            Mathf.Clamp((carMovement.wheelsVelocity / (customizer.currentCar.performance.acceleration * 0.4f)).Remap(0, 1, 0, frequencyMax), 0, frequencyMax * 2) + frequencyAdd;

        if (shakeTimer < 1)
        {
            shakeTimer += Time.deltaTime * shakeTime;

            cinemachineNoise.AmplitudeGain =
                Mathf.Lerp(cinemachineNoise.AmplitudeGain + shakePowered + amplitudeBase, amplitudeBase, Mathf.Clamp01(shakeTimer));
        }
    }
}