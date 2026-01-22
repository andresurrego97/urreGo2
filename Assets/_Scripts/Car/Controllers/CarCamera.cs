using Unity.Cinemachine;
using UnityEngine;

public class CarCamera : MonoBehaviour
{
    [SerializeField] private CarCustomizer customizer;
    [SerializeField] private CarMovement carMovement;
    [SerializeField] private CinemachineBasicMultiChannelPerlin cinemachineNoise;

    [Space]
    [SerializeField] private float frequencyMax;
    [SerializeField] private float frequencyAdd;

    private void Update()
    {
        if (customizer.currentRootReferences == null)
            return;

        cinemachineNoise.FrequencyGain =
            Mathf.Clamp((carMovement.wheelsVelocity / (customizer.currentCar.performance.acceleration * 0.4f)).Remap(0, 1, 0, frequencyMax), 0, frequencyMax) + frequencyAdd;
    }
}