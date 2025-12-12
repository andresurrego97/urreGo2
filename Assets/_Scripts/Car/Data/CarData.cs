using System.Collections.Generic;
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

public enum CarColorSlider
{
    Metallic,
    Smoothness,
    FlakeInfluence,
    DarkenInfluence
}

public struct CarColorsProperties
{
    public static readonly int BaseColor = Shader.PropertyToID("_BaseColor");
    public static readonly int TintA = Shader.PropertyToID("_TintA");
    public static readonly int TintB = Shader.PropertyToID("_TintB");
    public static readonly int TintC = Shader.PropertyToID("_TintC");
    public static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");

    public static Dictionary<(CarColorType, CarColorSlider), int> propertyMap =
    new()
    {
        { (CarColorType.Base, CarColorSlider.Metallic), Shader.PropertyToID("_BaseMetallic")},
        { (CarColorType.Base, CarColorSlider.Smoothness), Shader.PropertyToID("_BaseRoughness")},
        { (CarColorType.Base, CarColorSlider.FlakeInfluence), Shader.PropertyToID("_BaseFlakes")},
        { (CarColorType.Base, CarColorSlider.DarkenInfluence), Shader.PropertyToID("_DarkenBase")},

        { (CarColorType.TintA, CarColorSlider.Metallic), Shader.PropertyToID("_MetallicA")},
        { (CarColorType.TintA, CarColorSlider.Smoothness), Shader.PropertyToID("_RoughnessA")},
        { (CarColorType.TintA, CarColorSlider.FlakeInfluence), Shader.PropertyToID("_FlakesA")},
        { (CarColorType.TintA, CarColorSlider.DarkenInfluence), Shader.PropertyToID("_DarkenA")},

        { (CarColorType.TintB, CarColorSlider.Metallic), Shader.PropertyToID("_MetallicB")},
        { (CarColorType.TintB, CarColorSlider.Smoothness), Shader.PropertyToID("_RoughnessB")},
        { (CarColorType.TintB, CarColorSlider.FlakeInfluence), Shader.PropertyToID("_FlakesB")},
        { (CarColorType.TintB, CarColorSlider.DarkenInfluence), Shader.PropertyToID("_DarkenB")},

        { (CarColorType.TintC, CarColorSlider.Metallic), Shader.PropertyToID("_MetallicC")},
        { (CarColorType.TintC, CarColorSlider.Smoothness), Shader.PropertyToID("_RoughnessC")},
        { (CarColorType.TintC, CarColorSlider.FlakeInfluence), Shader.PropertyToID("_FlakesC")},
        { (CarColorType.TintC, CarColorSlider.DarkenInfluence), Shader.PropertyToID("_DarkenC")}
    };
};