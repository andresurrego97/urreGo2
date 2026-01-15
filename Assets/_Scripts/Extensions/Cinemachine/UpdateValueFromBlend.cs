using Cysharp.Threading.Tasks;
using System.Threading;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Rendering;

public class UpdateValueFromBlend : MonoBehaviour
{
    [SerializeField] private CinemachineBrain cinemachineBrain;
    [SerializeField] private Volume interiorVolume;

    private float time = 0;
    private CancellationTokenSource token;

    public void EnableBlend(bool on)
    {
        token?.Cancel();
        token = new CancellationTokenSource();

        if (on)
        {
            UpBlend().Forget();
        }
        else
        {
            DownBlend().Forget();
        }
    }

    private async UniTaskVoid UpBlend()
    {
        while (time < 1)
        {
            await UniTask.Delay(1, cancellationToken: token.Token);
            time += 0.01f;
            interiorVolume.weight = time;
        }
    }

    private async UniTaskVoid DownBlend()
    {
        while (time > 0)
        {
            await UniTask.Delay(1, cancellationToken: token.Token);
            time -= 0.01f;
            interiorVolume.weight = time;
        }
    }
}