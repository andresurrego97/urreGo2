using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public class CarSuspensionRaycasts : MonoBehaviour
{
    public static CarSuspensionRaycasts Instance;

    [Header("Config")]
    [SerializeField] private int maxCars = 30;
    [SerializeField] private int raysPerCar = 4;
    [SerializeField] private LayerMask suspensionMask;

    private NativeArray<RaycastCommand> commands;
    private NativeArray<RaycastHit> hits;
    private QueryParameters parameters;
    private int writeIndex;
    private int rayCount;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;

        rayCount = maxCars * raysPerCar;

        commands = new NativeArray<RaycastCommand>(rayCount, Allocator.Persistent);
        hits = new NativeArray<RaycastHit>(rayCount, Allocator.Persistent);
        parameters = new QueryParameters(suspensionMask, false, QueryTriggerInteraction.Ignore, false);
        //parameters = new QueryParameters(suspensionMask, true, QueryTriggerInteraction.Collide, true);
    }

    public int Reserve(int amount)
    {
        int start = writeIndex;
        writeIndex += amount;
        return start;
    }

    public void SetCommand(int index, Vector3 origin, Vector3 direction, float distance)
    {
        commands[index] = new RaycastCommand(
            origin,
            direction,
            parameters,
            distance * 2);
    }

    private void FixedUpdate()
    {
        //Debug.Log(writeIndex);

        if (writeIndex == 0)
            return;

        JobHandle handle = RaycastCommand.ScheduleBatch(commands, hits, maxCars, default);
        handle.Complete();
        //writeIndex = 0;
    }

    public RaycastHit GetHit(int index)
    {
        if (hits[index].collider != null)
        {
            Debug.DrawLine(commands[index].from, hits[index].point, Color.green);
        }
        else
        {
            Debug.DrawRay(commands[index].from, commands[index].direction * 10, Color.red);
        }

        return hits[index];
    }

    private void OnDestroy()
    {
        if (commands.IsCreated)
            commands.Dispose();
        if (hits.IsCreated)
            hits.Dispose();
    }
}