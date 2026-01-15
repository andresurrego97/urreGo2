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

    /// <summary>
    /// Cambiar segun cantidad de carros
    /// </summary>

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
    }

    public int Reserve(int amount = 4)
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
            distance);
    }

    private void Update()
    {
        if (writeIndex == 0)
            return;

        JobHandle handle = RaycastCommand.ScheduleBatch(commands, hits, maxCars);
        handle.Complete();
        writeIndex = 0;
    }

    public RaycastHit GetHit(int index)
    {
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