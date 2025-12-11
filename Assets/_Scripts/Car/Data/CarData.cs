using UnityEngine;

public enum CarPartType
{
    Body,
    BodyKit,
    Engine,
    SteeringWheel,
    Wheel
}

public enum CarColorType
{
    Base,
    TintA,
    TintB,
    TintC,
    Emission,
    All
}

public readonly struct CarColorsProperties
{
    public static readonly int BaseColor = Shader.PropertyToID("_BaseColor");
    public static readonly int BaseMetallic = Shader.PropertyToID("_BaseMetallic");
    public static readonly int BaseRoughness = Shader.PropertyToID("_BaseRoughness");
    public static readonly int BaseFlakes = Shader.PropertyToID("_BaseFlakes");
    public static readonly int DarkenBase = Shader.PropertyToID("_DarkenBase");
    public static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");

    public static readonly int TintA = Shader.PropertyToID("_TintA");
    public static readonly int TintB = Shader.PropertyToID("_TintB");
    public static readonly int TintC = Shader.PropertyToID("_TintC");

    public static readonly int MetallicA = Shader.PropertyToID("_MetallicA");
    public static readonly int MetallicB = Shader.PropertyToID("_MetallicB");
    public static readonly int MetallicC = Shader.PropertyToID("_MetallicC");

    public static readonly int RoughnessA = Shader.PropertyToID("_RoughnessA");
    public static readonly int RoughnessB = Shader.PropertyToID("_RoughnessB");
    public static readonly int RoughnessC = Shader.PropertyToID("_RoughnessC");

    public static readonly int FlakesA = Shader.PropertyToID("_FlakesA");
    public static readonly int FlakesB = Shader.PropertyToID("_FlakesB");
    public static readonly int FlakesC = Shader.PropertyToID("_FlakesC");

    public static readonly int DarkenA = Shader.PropertyToID("_DarkenA");
    public static readonly int DarkenB = Shader.PropertyToID("_DarkenB");
    public static readonly int DarkenC = Shader.PropertyToID("_DarkenC");
}