using Cysharp.Threading.Tasks;
using UnityEngine;

public class CarDetachedPart : MonoBehaviour
{
    [SerializeField] private new Collider collider;
    [SerializeField] private new Rigidbody rigidbody;

    [Space]
    [SerializeField] private float timeToDie = 5;

    private Vector3 detachePower;
    private float scale = 1;

    //private void Awake()
    //{
    //    collider.enabled = false;
    //    rigidbody.isKinematic = true;
    //}

    public void Detache(Vector3 rbPosition)
    {
        transform.SetParent(null);

        detachePower = rigidbody.position - rbPosition;
        detachePower *= 2.5f;
        detachePower.y *= 2;

        rigidbody.isKinematic = false;
        rigidbody.AddForceAtPosition(
            detachePower,
            rbPosition,
            ForceMode.Impulse);
        collider.enabled = true;

        WaitToDie().Forget();
    }

    private async UniTaskVoid WaitToDie()
    {
        await UniTask.WaitForSeconds(timeToDie, cancellationToken: destroyCancellationToken);

        collider.enabled = false;
        rigidbody.isKinematic = true;

        while (scale > 0)
        {
            scale -= Time.deltaTime;

            transform.localScale = new Vector3(scale, scale, scale);

            await UniTask.Yield(cancellationToken: destroyCancellationToken);
        }

        transform.localScale = Vector3.zero;
        gameObject.SetActive(false);
    }
}