using Cysharp.Threading.Tasks;
using System;
//using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public static class Extensions
{
    public static float Remap(this float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

    public static float Remap(this int value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

    public static int Remap(this int value, int from1, int to1, int from2, int to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

    //public static Vector3 InverseLossyScale(in Vector3 scale)
    //{
    //    return new Vector3(1f / scale.x, 1f / scale.y, 1f / scale.z);
    //}

    public static Transform FindChildRecursiveByName(this Transform source, string name, bool partial = false)
    {
        Transform child = source.FindByName(name, partial);

        for (int i = 0; i < source.childCount; i++)
        {
            if (child != null)
            {
                return child;
            }

            child = source.GetChild(i).FindChildRecursiveByName(name);
        }

        return child;
    }

    public static Transform FindByName(this Transform source, string name, bool partial = false)
    {
        for (int i = 0; i < source.childCount; i++)
        {
            if (partial)
            {
                if (source.GetChild(i).gameObject.name.Contains(name, StringComparison.InvariantCultureIgnoreCase))
                {
                    return source.GetChild(i);
                }
            }
            else
            {
                if (source.GetChild(i).gameObject.name == name)
                {
                    return source.GetChild(i);
                }
            }
        }

        return null;
    }

    public static async UniTask<T> AsyncLoadInstantiate<T>(string path) where T : UnityEngine.Object
    {
        return await AsyncInstantiate(
            await Resources.LoadAsync<T>(path) as T);
    }

    public static async UniTask<T> AsyncInstantiate<T>(T ob, Transform parent = null, CancellationToken cancellationToken = default) where T : UnityEngine.Object
    {
        return (await UnityEngine.Object.InstantiateAsync(
            ob,
            new InstantiateParameters { parent = parent, worldSpace = false },
            cancellationToken))[0];
    }

    public static async UniTask<T> AsyncInstantiate<T>(T ob, Vector3 position, Quaternion rotation, Transform parent = null) where T : UnityEngine.Object
    {
        return (await UnityEngine.Object.InstantiateAsync(
            ob,
            position,
            rotation,
            new InstantiateParameters { parent = parent, worldSpace = false }))[0];
    }

    public static async UniTask<T[]> AsyncInstantiate<T>(T ob, int count, Transform parent = null) where T : UnityEngine.Object
    {
        return await UnityEngine.Object.InstantiateAsync(
            ob,
            count,
            new InstantiateParameters { parent = parent, worldSpace = false });
    }

    public static async UniTask<T[]> AsyncInstantiate<T>(T ob, int count, Vector3[] positions, Quaternion[] rotations, Transform parent = null) where T : UnityEngine.Object
    {
        return await UnityEngine.Object.InstantiateAsync(
            ob,
            count,
            positions.AsSpan(),
            rotations.AsSpan(),
            new InstantiateParameters { parent = parent, worldSpace = false });
    }
}

//public class WeightedRandomSelector<T>
//{
//    private readonly List<(T item, int weight)> items;
//    private int totalWeight;
//    private int randomValue;
//    private int cumulativeWeight;

//    public WeightedRandomSelector()
//    {
//        items = new List<(T, int)>();
//        totalWeight = 0;
//    }

//    public void AddItem(T item, int weight)
//    {
//        if (weight <= 0)
//        {
//            return;
//        }

//        items.Add((item, weight));
//        totalWeight += weight;
//    }

//    public T GetPonderedItem()
//    {
//        randomValue = UnityEngine.Random.Range(0, totalWeight);
//        cumulativeWeight = 0;

//        for (int i = 0; i < items.Count; i++)
//        {
//            cumulativeWeight += items[i].weight;
//            if (randomValue < cumulativeWeight)
//            {
//                return items[i].item;
//            }
//        }

//        return default;
//    }
//}