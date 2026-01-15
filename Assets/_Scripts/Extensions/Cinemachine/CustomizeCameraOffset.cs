using Cysharp.Threading.Tasks;
using Unity.Cinemachine;
using UnityEngine;

public class CustomizeCameraOffset : MonoBehaviour
{
    [SerializeField] private CinemachineRotationComposer rotationComposer;
    [SerializeField] private Vector2 normalPosition = new(0, 0.05f);
    [SerializeField] private Vector2 offsetPosition = new(0.18f, 0);

    private bool offset = true;
    private float time = 0;

    private void Awake()
    {
        rotationComposer.Composition.ScreenPosition = offsetPosition;
    }

    public void Offset()
    {
        offset = !offset;

        Move(offset).Forget();
    }

    private async UniTaskVoid Move(bool up)
    {
        time = 0;

        while (time < 1)
        {
            time += 0.01f;

            rotationComposer.Composition.ScreenPosition =
                Vector2.Lerp(rotationComposer.Composition.ScreenPosition, up ? offsetPosition : normalPosition, time);

            await UniTask.NextFrame();
        }

        rotationComposer.Composition.ScreenPosition = up ? offsetPosition : normalPosition;
    }
}