using Cysharp.Threading.Tasks;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CarCustomizer : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CarPartsList partsList;
    [SerializeField] private CarColors colorsList;
    [SerializeField] private Transform root;

    [Header("UI")]
    [Header("Parts")]
    [SerializeField] private TextMeshProUGUI title_body;
    [SerializeField] private TextMeshProUGUI title_bodyKit;
    [SerializeField] private TextMeshProUGUI title_steeringWheel;
    [SerializeField] private TextMeshProUGUI title_wheels;


    [Header("Colors")]
    [Header("Base color")]
    [SerializeField] private TextMeshProUGUI title_baseColor;
    [SerializeField] private Slider slider_baseColor_metallic;
    [SerializeField] private Slider slider_baseColor_smoothness;
    [SerializeField] private Slider slider_baseColor_flakeInflience;
    [SerializeField] private Slider slider_baseColor_darkenInfluence;
    [SerializeField] private TextMeshProUGUI number_baseColor_metallic;
    [SerializeField] private TextMeshProUGUI number_baseColor_smoothness;
    [SerializeField] private TextMeshProUGUI number_baseColor_flakeInflience;
    [SerializeField] private TextMeshProUGUI number_baseColor_darkenInfluence;

    [Header("Tint A")]
    [SerializeField] private TextMeshProUGUI title_tintA;
    [SerializeField] private Slider slider_tintA_metallic;
    [SerializeField] private Slider slider_tintA_smoothness;
    [SerializeField] private Slider slider_tintA_flakeInflience;
    [SerializeField] private Slider slider_tintA_darkenInfluence;
    [SerializeField] private TextMeshProUGUI number_tintA_metallic;
    [SerializeField] private TextMeshProUGUI number_tintA_smoothness;
    [SerializeField] private TextMeshProUGUI number_tintA_flakeInflience;
    [SerializeField] private TextMeshProUGUI number_tintA_darkenInfluence;

    [Header("Tint B")]
    [SerializeField] private TextMeshProUGUI title_tintB;
    [SerializeField] private Slider slider_tintB_metallic;
    [SerializeField] private Slider slider_tintB_smoothness;
    [SerializeField] private Slider slider_tintB_flakeInflience;
    [SerializeField] private Slider slider_tintB_darkenInfluence;
    [SerializeField] private TextMeshProUGUI number_tintB_metallic;
    [SerializeField] private TextMeshProUGUI number_tintB_smoothness;
    [SerializeField] TextMeshProUGUI number_tintB_flakeInflience;
    [SerializeField] TextMeshProUGUI number_tintB_darkenInfluence;

    [Header("Tint C")]
    [SerializeField] private TextMeshProUGUI title_tintC;
    [SerializeField] private Slider slider_tintC_metallic;
    [SerializeField] private Slider slider_tintC_smoothness;
    [SerializeField] private Slider slider_tintC_flakeInflience;
    [SerializeField] private Slider slider_tintC_darkenInfluence;
    [SerializeField] TextMeshProUGUI number_tintC_metallic;
    [SerializeField] TextMeshProUGUI number_tintC_smoothness;
    [SerializeField] TextMeshProUGUI number_tintC_flakeInflience;
    [SerializeField] TextMeshProUGUI number_tintC_darkenInfluence;

    [Space]
    [SerializeField] private TextMeshProUGUI title_emission;
    [SerializeField] private TextMeshProUGUI title_decal;

    private int currentPart_body = -1;
    private bool currentPart_bodyKit = false;
    private int currentPart_steeringWheel = -1;
    private int currentPart_wheels = -1;

    private int currentColor_base = 0;
    private int currentColor_tintA = 0;
    private int currentColor_tintB = 0;
    private int currentColor_tintC = 0;
    private int currentColor_emission = 0;

    private int currentDecal = -1;

    private CarRootReferences currentRootReferences;

    private void Awake()
    {
        title_body.text = "None";
        title_bodyKit.text = "None";
        title_steeringWheel.text = "None";
        title_wheels.text = "None";

        title_baseColor.text = colorsList.colors[0].name;
        title_tintA.text = colorsList.colors[0].name;
        title_tintB.text = colorsList.colors[0].name;
        title_tintC.text = colorsList.colors[0].name;
        title_emission.text = colorsList.colors[0].name;

        slider_baseColor_metallic.value = 0;
        slider_baseColor_smoothness.value = 0;
        slider_baseColor_flakeInflience.value = 0;
        slider_baseColor_darkenInfluence.value = 0;
        number_baseColor_metallic.text = "0";
        number_baseColor_smoothness.text = "0";
        number_baseColor_flakeInflience.text = "0";
        number_baseColor_darkenInfluence.text = "0";

        slider_tintA_metallic.value = 0;
        slider_tintA_smoothness.value = 0;
        slider_tintA_flakeInflience.value = 0;
        slider_tintA_darkenInfluence.value = 0;
        number_tintA_metallic.text = "0";
        number_tintA_smoothness.text = "0";
        number_tintA_flakeInflience.text = "0";
        number_tintA_darkenInfluence.text = "0";

        slider_tintB_metallic.value = 0;
        slider_tintB_smoothness.value = 0;
        slider_tintB_flakeInflience.value = 0;
        slider_tintB_darkenInfluence.value = 0;
        number_tintB_metallic.text = "0";
        number_tintB_smoothness.text = "0";
        number_tintB_flakeInflience.text = "0";
        number_tintB_darkenInfluence.text = "0";

        slider_tintC_metallic.value = 0;
        slider_tintC_smoothness.value = 0;
        slider_tintC_flakeInflience.value = 0;
        slider_tintC_darkenInfluence.value = 0;
        number_tintC_metallic.text = "0";
        number_tintC_smoothness.text = "0";
        number_tintC_flakeInflience.text = "0";
        number_tintC_darkenInfluence.text = "0";

        title_decal.text = "None";
    }

    #region Parts

    #region Body
    public void Parts_Body_Back()
    {
        currentPart_body--;

        if (currentPart_body < 0)
        {
            currentPart_body = partsList.cars.Length - 1;
        }

        Instantiate_Body().Forget();
    }

    public void Parts_Body_Next()
    {
        currentPart_body++;

        if (currentPart_body > partsList.cars.Length - 1)
        {
            currentPart_body = 0;
        }

        Instantiate_Body().Forget();
    }

    private async UniTaskVoid Instantiate_Body()
    {
        title_body.text = partsList.cars[currentPart_body].name;

        for (int i = 0; i < root.childCount; i++)
        {
            Destroy(root.GetChild(i).gameObject);
        }

        currentRootReferences =
            (await Extensions.AsyncInstantiate(partsList.cars[currentPart_body].body.part, root)).GetComponent<CarRootReferences>();

        if (currentRootReferences.renderer_body != null)
            currentRootReferences.material_body = currentRootReferences.renderer_body.material;
        if (currentRootReferences.renderer_chassis != null)
            currentRootReferences.material_chassis = currentRootReferences.renderer_chassis.material;
        if (currentRootReferences.renderer_dash != null)
            currentRootReferences.material_dash = currentRootReferences.renderer_dash.material;
        if (currentRootReferences.renderer_emissive != null)
            currentRootReferences.material_emissive = currentRootReferences.renderer_emissive.material;
        if (currentRootReferences.renderer_glass != null)
            currentRootReferences.material_glass = currentRootReferences.renderer_glass.material;
        if (currentRootReferences.renderer_interior != null)
            currentRootReferences.material_interior = currentRootReferences.renderer_interior.material;

        //currentDecal = -1;
        SetColor_BodyParts(CarColorType.All);

        currentPart_bodyKit = false;
        Parts_BodyKit_Next();

        currentPart_wheels = -1;
        currentPart_wheels = Array.IndexOf(partsList.comboWheels, partsList.cars[currentPart_body].wheel) - 1;
        Parts_Wheels_Next();

        Engine_Next().Forget();

        currentPart_steeringWheel = -1;
        currentPart_steeringWheel = Array.IndexOf(partsList.steeringWheels, partsList.cars[currentPart_body].steeringWheel) - 1;
        Parts_SteeringWheel_Next();
    }
    #endregion

    #region Body Kit
    public void Parts_BodyKit_Next()
    {
        currentPart_bodyKit = !currentPart_bodyKit;

        if (partsList.cars[currentPart_body].bodyKit != null)
        {
            if (currentPart_bodyKit)
            {
                title_bodyKit.text = partsList.cars[currentPart_body].bodyKit.name;
                Instantiate_BodyKit().Forget();
            }
            else
            {
                title_bodyKit.text = "None";
                Destroy(currentRootReferences.root_bodyKit.GetChild(0).gameObject);
            }
        }
        else
        {
            title_bodyKit.text = "Default";
        }
    }

    private async UniTaskVoid Instantiate_BodyKit()
    {
        //title_bodyKit.text = list.cars[currentBody].bodyKit.name;

        currentRootReferences.renderer_bodyKit =
            (await Extensions.AsyncInstantiate(partsList.cars[currentPart_body].bodyKit.part, currentRootReferences.root_bodyKit)).GetComponentInChildren<Renderer>();

        currentRootReferences.material_bodyKit = currentRootReferences.renderer_bodyKit.material;
        SetColor_BodyKitPart(CarColorType.All);
    }
    #endregion

    #region Wheels
    public void Parts_Wheels_Back()
    {
        currentPart_wheels--;

        if (currentPart_wheels < 0)
        {
            currentPart_wheels = partsList.cars.Length - 1;
        }

        Instantiate_Wheels().Forget();
    }

    public void Parts_Wheels_Next()
    {
        if (currentPart_wheels == -2)
        {
            title_wheels.text = "Default";
            return;
        }

        currentPart_wheels++;

        if (currentPart_wheels > partsList.cars.Length - 1)
        {
            currentPart_wheels = 0;
        }

        title_wheels.text = partsList.comboWheels[currentPart_wheels].name;

        Instantiate_Wheels().Forget();
    }

    private async UniTaskVoid Instantiate_Wheels()
    {
        // front
        for (int i = 0; i < currentRootReferences.root_frontWheels.Length; i++)
        {
            if (currentRootReferences.root_frontWheels[i].childCount == 1)
            {
                Destroy(currentRootReferences.root_frontWheels[i].GetChild(0).gameObject);
            }
        }

        if (partsList.comboWheels[currentPart_wheels].firstPart != null)
        {
            currentRootReferences.renderer_frontWheels = new MeshRenderer[currentRootReferences.root_frontWheels.Length];
            for (int i = 0; i < currentRootReferences.root_frontWheels.Length; i++)
            {
                currentRootReferences.renderer_frontWheels[i] =
                    (await Extensions.AsyncInstantiate(partsList.comboWheels[currentPart_wheels].firstPart.part, currentRootReferences.root_frontWheels[i])).GetComponentInChildren<Renderer>();

                if (i == 0)
                {
                    currentRootReferences.material_wheels = currentRootReferences.renderer_frontWheels[0].material;
                }
                else
                {
                    currentRootReferences.renderer_frontWheels[i].material = currentRootReferences.material_wheels;
                }
            }
        }
        else
        {
            currentRootReferences.renderer_frontWheels = null;
        }

        // back
        for (int i = 0; i < currentRootReferences.root_backWheels.Length; i++)
        {
            if (currentRootReferences.root_backWheels[i].childCount == 1)
            {
                Destroy(currentRootReferences.root_backWheels[i].GetChild(0).gameObject);
            }
        }

        if (partsList.comboWheels[currentPart_wheels].secondPart != null)
        {
            currentRootReferences.renderer_backWheels = new MeshRenderer[currentRootReferences.root_backWheels.Length];
            for (int i = 0; i < currentRootReferences.root_backWheels.Length; i++)
            {
                currentRootReferences.renderer_backWheels[i] =
                    (await Extensions.AsyncInstantiate(partsList.comboWheels[currentPart_wheels].secondPart.part, currentRootReferences.root_backWheels[i])).GetComponentInChildren<Renderer>();

                currentRootReferences.renderer_backWheels[i].material = currentRootReferences.material_wheels;
            }
        }
        else if (partsList.comboWheels[currentPart_wheels].firstPart != null)
        {
            currentRootReferences.renderer_backWheels = new MeshRenderer[currentRootReferences.root_backWheels.Length];
            for (int i = 0; i < currentRootReferences.root_backWheels.Length; i++)
            {
                currentRootReferences.renderer_backWheels[i] =
                    (await Extensions.AsyncInstantiate(partsList.comboWheels[currentPart_wheels].firstPart.part, currentRootReferences.root_backWheels[i])).GetComponentInChildren<Renderer>();

                currentRootReferences.renderer_backWheels[i].material = currentRootReferences.material_wheels;
            }
        }
        else
        {
            currentRootReferences.renderer_frontWheels = null;
        }

        // brakes
        for (int i = 0; i < currentRootReferences.root_brakes.Length; i++)
        {
            if (currentRootReferences.root_brakes[i].childCount == 1)
            {
                Destroy(currentRootReferences.root_brakes[i].GetChild(0).gameObject);
            }
        }

        if (partsList.comboWheels[currentPart_wheels].thirdPart != null)
        {
            currentRootReferences.renderer_brakes = new MeshRenderer[currentRootReferences.root_brakes.Length];
            for (int i = 0; i < currentRootReferences.root_brakes.Length; i++)
            {
                currentRootReferences.renderer_brakes[i] =
                    (await Extensions.AsyncInstantiate(partsList.comboWheels[currentPart_wheels].thirdPart.part, currentRootReferences.root_brakes[i])).GetComponentInChildren<Renderer>();

                currentRootReferences.renderer_brakes[i].material = currentRootReferences.material_wheels;
            }
        }
        else
        {
            currentRootReferences.renderer_brakes = null;
        }

        SetColor_WheelsPart(CarColorType.All);
    }
    #endregion

    #region Engine
    private async UniTaskVoid Engine_Next()
    {
        currentRootReferences.renderer_engine =
        (await Extensions.AsyncInstantiate(partsList.cars[currentPart_body].engine.part, currentRootReferences.root_engine)).GetComponentInChildren<Renderer>();

        currentRootReferences.material_engine = currentRootReferences.renderer_engine.material;
        SetColor_EnginePart(CarColorType.All);
    }
    #endregion

    #region Steering Wheel
    public void Parts_SteeringWheel_Back()
    {
        currentPart_steeringWheel--;

        if (currentPart_steeringWheel < 0)
        {
            currentPart_steeringWheel = partsList.cars.Length - 1;
        }

        Instantiate_SteeringWheel().Forget();
    }

    public void Parts_SteeringWheel_Next()
    {
        currentPart_steeringWheel++;

        if (currentPart_steeringWheel > partsList.cars.Length - 1)
        {
            currentPart_steeringWheel = 0;
        }

        title_steeringWheel.text = partsList.steeringWheels[currentPart_steeringWheel].name;

        Instantiate_SteeringWheel().Forget();
    }

    private async UniTaskVoid Instantiate_SteeringWheel()
    {
        if (currentRootReferences.root_steeringWheel.childCount == 1)
        {
            Destroy(currentRootReferences.root_steeringWheel.GetChild(0).gameObject);
        }

        currentRootReferences.renderer_steeringWheel =
            (await Extensions.AsyncInstantiate(partsList.steeringWheels[currentPart_steeringWheel].part, currentRootReferences.root_steeringWheel)).GetComponentInChildren<Renderer>();

        currentRootReferences.material_steeringWheel = currentRootReferences.renderer_steeringWheel.material;
        SetColor_SteeringWheelPart(CarColorType.All);
    }
    #endregion

    #endregion

    #region Colors

    #region Base Color
    public void Colors_Base_Back()
    {
        currentColor_base--;

        if (currentColor_base < 0)
        {
            currentColor_base = colorsList.colors.Length - 1;
        }

        SetColor_Base();
    }

    public void Colors_Base_Next()
    {
        currentColor_base++;

        if (currentColor_base > colorsList.colors.Length - 1)
        {
            currentColor_base = 0;
        }

        SetColor_Base();
    }

    private void SetColor_Base()
    {
        title_baseColor.text = colorsList.colors[currentColor_base].name;

        SetColor_BodyParts(CarColorType.Base);
        SetColor_BodyKitPart(CarColorType.Base);
        SetColor_EnginePart(CarColorType.Base);
        SetColor_SteeringWheelPart(CarColorType.Base);
        SetColor_WheelsPart(CarColorType.Base);
    }
    #endregion

    #region Tint A
    public void Colors_TintA_Back()
    {
        currentColor_tintA--;

        if (currentColor_tintA < 0)
        {
            currentColor_tintA = colorsList.colors.Length - 1;
        }

        SetColor_TintA();
    }

    public void Colors_TintA_Next()
    {
        currentColor_tintA++;

        if (currentColor_tintA > colorsList.colors.Length - 1)
        {
            currentColor_tintA = 0;
        }

        SetColor_TintA();
    }

    private void SetColor_TintA()
    {
        title_tintA.text = colorsList.colors[currentColor_tintA].name;

        SetColor_BodyParts(CarColorType.TintA);
        SetColor_BodyKitPart(CarColorType.TintA);
        SetColor_EnginePart(CarColorType.TintA);
        SetColor_SteeringWheelPart(CarColorType.TintA);
        SetColor_WheelsPart(CarColorType.TintA);
    }
    #endregion

    #region Tint B
    public void Colors_TintB_Back()
    {
        currentColor_tintB--;

        if (currentColor_tintB < 0)
        {
            currentColor_tintB = colorsList.colors.Length - 1;
        }

        SetColor_TintB();
    }

    public void Colors_TintB_Next()
    {
        currentColor_tintB++;

        if (currentColor_tintB > colorsList.colors.Length - 1)
        {
            currentColor_tintB = 0;
        }

        SetColor_TintB();
    }

    private void SetColor_TintB()
    {
        title_tintB.text = colorsList.colors[currentColor_tintB].name;

        SetColor_BodyParts(CarColorType.TintB);
        SetColor_BodyKitPart(CarColorType.TintB);
        SetColor_EnginePart(CarColorType.TintB);
        SetColor_SteeringWheelPart(CarColorType.TintB);
        SetColor_WheelsPart(CarColorType.TintB);
    }
    #endregion

    #region Tint C
    public void Colors_TintC_Back()
    {
        currentColor_tintC--;

        if (currentColor_tintC < 0)
        {
            currentColor_tintC = colorsList.colors.Length - 1;
        }

        SetColor_TintC();
    }

    public void Colors_TintC_Next()
    {
        currentColor_tintC++;

        if (currentColor_tintC > colorsList.colors.Length - 1)
        {
            currentColor_tintC = 0;
        }

        SetColor_TintC();
    }

    private void SetColor_TintC()
    {
        title_tintC.text = colorsList.colors[currentColor_tintC].name;

        SetColor_BodyParts(CarColorType.TintC);
        SetColor_BodyKitPart(CarColorType.TintC);
        SetColor_EnginePart(CarColorType.TintC);
        SetColor_SteeringWheelPart(CarColorType.TintC);
        SetColor_WheelsPart(CarColorType.TintC);
    }
    #endregion

    #region Emission
    public void Colors_Emission_Back()
    {
        currentColor_emission--;

        if (currentColor_emission < 0)
        {
            currentColor_emission = colorsList.colors.Length - 1;
        }

        SetColor_Emission();
    }

    public void Colors_Emission_Next()
    {
        currentColor_emission++;

        if (currentColor_emission > colorsList.colors.Length - 1)
        {
            currentColor_emission = 0;
        }

        SetColor_Emission();
    }

    private void SetColor_Emission()
    {
        title_emission.text = colorsList.colors[currentColor_emission].name;

        SetColor_BodyParts(CarColorType.Emission);
        SetColor_BodyKitPart(CarColorType.Emission);
        SetColor_EnginePart(CarColorType.Emission);
        SetColor_SteeringWheelPart(CarColorType.Emission);
        SetColor_WheelsPart(CarColorType.Emission);
    }
    #endregion

    #region Set Parts Colors
    private void SetColor_BodyParts(CarColorType type)
    {
        switch (type)
        {
            case CarColorType.Base:
                if (currentRootReferences.material_body != null)
                    currentRootReferences.material_body.SetColor(CarColorsProperties.BaseColor, colorsList.colors[currentColor_base].color);
                if (currentRootReferences.material_chassis != null)
                    currentRootReferences.material_chassis.SetColor(CarColorsProperties.BaseColor, colorsList.colors[currentColor_base].color);
                if (currentRootReferences.material_dash != null)
                    currentRootReferences.material_dash.SetColor(CarColorsProperties.BaseColor, colorsList.colors[currentColor_base].color);
                if (currentRootReferences.material_emissive != null)
                    currentRootReferences.material_emissive.SetColor(CarColorsProperties.BaseColor, colorsList.colors[currentColor_base].color);
                if (currentRootReferences.material_glass != null)
                    currentRootReferences.material_glass.SetColor(CarColorsProperties.BaseColor, colorsList.colors[currentColor_base].color);
                if (currentRootReferences.material_interior != null)
                    currentRootReferences.material_interior.SetColor(CarColorsProperties.BaseColor, colorsList.colors[currentColor_base].color);
                break;

            case CarColorType.TintA:
                if (currentRootReferences.material_body != null)
                    currentRootReferences.material_body.SetColor(CarColorsProperties.TintA, colorsList.colors[currentColor_tintA].color);
                if (currentRootReferences.material_chassis != null)
                    currentRootReferences.material_chassis.SetColor(CarColorsProperties.TintA, colorsList.colors[currentColor_tintA].color);
                if (currentRootReferences.material_dash != null)
                    currentRootReferences.material_dash.SetColor(CarColorsProperties.TintA, colorsList.colors[currentColor_tintA].color);
                if (currentRootReferences.material_emissive != null)
                    currentRootReferences.material_emissive.SetColor(CarColorsProperties.TintA, colorsList.colors[currentColor_tintA].color);
                if (currentRootReferences.material_glass != null)
                    currentRootReferences.material_glass.SetColor(CarColorsProperties.TintA, colorsList.colors[currentColor_tintA].color);
                if (currentRootReferences.material_interior != null)
                    currentRootReferences.material_interior.SetColor(CarColorsProperties.TintA, colorsList.colors[currentColor_tintA].color);
                break;

            case CarColorType.TintB:
                if (currentRootReferences.material_body != null)
                    currentRootReferences.material_body.SetColor(CarColorsProperties.TintB, colorsList.colors[currentColor_tintB].color);
                if (currentRootReferences.material_chassis != null)
                    currentRootReferences.material_chassis.SetColor(CarColorsProperties.TintB, colorsList.colors[currentColor_tintB].color);
                if (currentRootReferences.material_dash != null)
                    currentRootReferences.material_dash.SetColor(CarColorsProperties.TintB, colorsList.colors[currentColor_tintB].color);
                if (currentRootReferences.material_emissive != null)
                    currentRootReferences.material_emissive.SetColor(CarColorsProperties.TintB, colorsList.colors[currentColor_tintB].color);
                if (currentRootReferences.material_glass != null)
                    currentRootReferences.material_glass.SetColor(CarColorsProperties.TintB, colorsList.colors[currentColor_tintB].color);
                if (currentRootReferences.material_interior != null)
                    currentRootReferences.material_interior.SetColor(CarColorsProperties.TintB, colorsList.colors[currentColor_tintB].color);
                break;

            case CarColorType.TintC:
                if (currentRootReferences.material_body != null)
                    currentRootReferences.material_body.SetColor(CarColorsProperties.TintC, colorsList.colors[currentColor_tintC].color);
                if (currentRootReferences.material_chassis != null)
                    currentRootReferences.material_chassis.SetColor(CarColorsProperties.TintC, colorsList.colors[currentColor_tintC].color);
                if (currentRootReferences.material_dash != null)
                    currentRootReferences.material_dash.SetColor(CarColorsProperties.TintC, colorsList.colors[currentColor_tintC].color);
                if (currentRootReferences.material_emissive != null)
                    currentRootReferences.material_emissive.SetColor(CarColorsProperties.TintC, colorsList.colors[currentColor_tintC].color);
                if (currentRootReferences.material_glass != null)
                    currentRootReferences.material_glass.SetColor(CarColorsProperties.TintC, colorsList.colors[currentColor_tintC].color);
                if (currentRootReferences.material_interior != null)
                    currentRootReferences.material_interior.SetColor(CarColorsProperties.TintC, colorsList.colors[currentColor_tintC].color);
                break;

            case CarColorType.Emission:
                if (currentRootReferences.material_body != null)
                    currentRootReferences.material_body.SetColor(CarColorsProperties.EmissionColor, colorsList.colors[currentColor_emission].color);
                if (currentRootReferences.material_chassis != null)
                    currentRootReferences.material_chassis.SetColor(CarColorsProperties.EmissionColor, colorsList.colors[currentColor_emission].color);
                if (currentRootReferences.material_dash != null)
                    currentRootReferences.material_dash.SetColor(CarColorsProperties.EmissionColor, colorsList.colors[currentColor_emission].color);
                if (currentRootReferences.material_emissive != null)
                    currentRootReferences.material_emissive.SetColor(CarColorsProperties.EmissionColor, colorsList.colors[currentColor_emission].color);
                if (currentRootReferences.material_glass != null)
                    currentRootReferences.material_glass.SetColor(CarColorsProperties.EmissionColor, colorsList.colors[currentColor_emission].color);
                if (currentRootReferences.material_interior != null)
                    currentRootReferences.material_interior.SetColor(CarColorsProperties.EmissionColor, colorsList.colors[currentColor_emission].color);
                break;

            case CarColorType.All:
                SetColor_BodyParts(CarColorType.Base);
                SetColor_BodyParts(CarColorType.TintA);
                SetColor_BodyParts(CarColorType.TintB);
                SetColor_BodyParts(CarColorType.TintC);
                SetColor_BodyParts(CarColorType.Emission);
                break;
        }
    }

    private void SetColor_BodyKitPart(CarColorType type)
    {
        if (currentRootReferences.material_bodyKit == null)
            return;

        switch (type)
        {
            case CarColorType.Base:
                currentRootReferences.material_bodyKit.SetColor(CarColorsProperties.BaseColor, colorsList.colors[currentColor_base].color);
                break;

            case CarColorType.TintA:
                currentRootReferences.material_bodyKit.SetColor(CarColorsProperties.TintA, colorsList.colors[currentColor_tintA].color);
                break;

            case CarColorType.TintB:
                currentRootReferences.material_bodyKit.SetColor(CarColorsProperties.TintB, colorsList.colors[currentColor_tintB].color);
                break;

            case CarColorType.TintC:
                currentRootReferences.material_bodyKit.SetColor(CarColorsProperties.TintC, colorsList.colors[currentColor_tintC].color);
                break;

            case CarColorType.Emission:
                currentRootReferences.material_bodyKit.SetColor(CarColorsProperties.EmissionColor, colorsList.colors[currentColor_emission].color);
                break;

            case CarColorType.All:
                SetColor_BodyKitPart(CarColorType.Base);
                SetColor_BodyKitPart(CarColorType.TintA);
                SetColor_BodyKitPart(CarColorType.TintB);
                SetColor_BodyKitPart(CarColorType.TintC);
                SetColor_BodyKitPart(CarColorType.Emission);
                break;
        }
    }

    private void SetColor_EnginePart(CarColorType type)
    {
        if (currentRootReferences.material_engine == null)
            return;

        switch (type)
        {
            case CarColorType.Base:
                currentRootReferences.material_engine.SetColor(CarColorsProperties.BaseColor, colorsList.colors[currentColor_base].color);
                break;

            case CarColorType.TintA:
                currentRootReferences.material_engine.SetColor(CarColorsProperties.TintA, colorsList.colors[currentColor_tintA].color);
                break;

            case CarColorType.TintB:
                currentRootReferences.material_engine.SetColor(CarColorsProperties.TintB, colorsList.colors[currentColor_tintB].color);
                break;

            case CarColorType.TintC:
                currentRootReferences.material_engine.SetColor(CarColorsProperties.TintC, colorsList.colors[currentColor_tintC].color);
                break;

            case CarColorType.Emission:
                currentRootReferences.material_engine.SetColor(CarColorsProperties.EmissionColor, colorsList.colors[currentColor_emission].color);
                break;

            case CarColorType.All:
                SetColor_EnginePart(CarColorType.Base);
                SetColor_EnginePart(CarColorType.TintA);
                SetColor_EnginePart(CarColorType.TintB);
                SetColor_EnginePart(CarColorType.TintC);
                SetColor_EnginePart(CarColorType.Emission);
                break;
        }
    }

    private void SetColor_SteeringWheelPart(CarColorType type)
    {
        if (currentRootReferences.material_steeringWheel == null)
            return;

        switch (type)
        {
            case CarColorType.Base:
                currentRootReferences.material_steeringWheel.SetColor(CarColorsProperties.BaseColor, colorsList.colors[currentColor_base].color);
                break;

            case CarColorType.TintA:
                currentRootReferences.material_steeringWheel.SetColor(CarColorsProperties.TintA, colorsList.colors[currentColor_tintA].color);
                break;

            case CarColorType.TintB:
                currentRootReferences.material_steeringWheel.SetColor(CarColorsProperties.TintB, colorsList.colors[currentColor_tintB].color);
                break;

            case CarColorType.TintC:
                currentRootReferences.material_steeringWheel.SetColor(CarColorsProperties.TintC, colorsList.colors[currentColor_tintC].color);
                break;

            case CarColorType.Emission:
                currentRootReferences.material_steeringWheel.SetColor(CarColorsProperties.EmissionColor, colorsList.colors[currentColor_emission].color);
                break;

            case CarColorType.All:
                SetColor_SteeringWheelPart(CarColorType.Base);
                SetColor_SteeringWheelPart(CarColorType.TintA);
                SetColor_SteeringWheelPart(CarColorType.TintB);
                SetColor_SteeringWheelPart(CarColorType.TintC);
                SetColor_SteeringWheelPart(CarColorType.Emission);
                break;
        }
    }

    private void SetColor_WheelsPart(CarColorType type)
    {
        if (currentRootReferences.material_wheels == null)
            return;

        switch (type)
        {
            case CarColorType.Base:
                currentRootReferences.material_wheels.SetColor(CarColorsProperties.BaseColor, colorsList.colors[currentColor_base].color);
                break;

            case CarColorType.TintA:
                currentRootReferences.material_wheels.SetColor(CarColorsProperties.TintA, colorsList.colors[currentColor_tintA].color);
                break;

            case CarColorType.TintB:
                currentRootReferences.material_wheels.SetColor(CarColorsProperties.TintB, colorsList.colors[currentColor_tintB].color);
                break;

            case CarColorType.TintC:
                currentRootReferences.material_wheels.SetColor(CarColorsProperties.TintC, colorsList.colors[currentColor_tintC].color);
                break;

            case CarColorType.Emission:
                currentRootReferences.material_wheels.SetColor(CarColorsProperties.EmissionColor, colorsList.colors[currentColor_emission].color);
                break;

            case CarColorType.All:
                SetColor_WheelsPart(CarColorType.Base);
                SetColor_WheelsPart(CarColorType.TintA);
                SetColor_WheelsPart(CarColorType.TintB);
                SetColor_WheelsPart(CarColorType.TintC);
                SetColor_WheelsPart(CarColorType.Emission);
                break;
        }
    }
    #endregion

    #endregion
}