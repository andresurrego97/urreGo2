using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Rendering;

public class UpdateValueFromBlend : MonoBehaviour
{
    [SerializeField] private CinemachineBrain cinemachineBrain;
    [SerializeField] private Volume interiorVolume;

    private bool up = true;

    public void EnableBlend(bool on)
    {
        up = on;
    }

    private void Update()
    {
        if (!cinemachineBrain.IsBlending)
            return;

        if (up)
        {
            interiorVolume.weight = cinemachineBrain.ActiveBlend.TimeInBlend;
        }
        else
        {
            interiorVolume.weight = cinemachineBrain.ActiveBlend.TimeInBlend.Remap(0, 1, 1, 0);
        }
    }
}