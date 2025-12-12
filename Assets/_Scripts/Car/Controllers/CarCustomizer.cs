using Cysharp.Threading.Tasks;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class CarCustomizer : MonoBehaviour
{
    #region Serialize Fields
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
    [SerializeField] private Slider slider_baseColor_flakeInfluence;
    [SerializeField] private Slider slider_baseColor_darkenInfluence;
    [SerializeField] private TextMeshProUGUI number_baseColor_metallic;
    [SerializeField] private TextMeshProUGUI number_baseColor_smoothness;
    [SerializeField] private TextMeshProUGUI number_baseColor_flakeInfluence;
    [SerializeField] private TextMeshProUGUI number_baseColor_darkenInfluence;

    [Header("Tint A")]
    [SerializeField] private TextMeshProUGUI title_tintA;
    [SerializeField] private Slider slider_tintA_metallic;
    [SerializeField] private Slider slider_tintA_smoothness;
    [SerializeField] private Slider slider_tintA_flakeInfluence;
    [SerializeField] private Slider slider_tintA_darkenInfluence;
    [SerializeField] private TextMeshProUGUI number_tintA_metallic;
    [SerializeField] private TextMeshProUGUI number_tintA_smoothness;
    [SerializeField] private TextMeshProUGUI number_tintA_flakeInfluence;
    [SerializeField] private TextMeshProUGUI number_tintA_darkenInfluence;

    [Header("Tint B")]
    [SerializeField] private TextMeshProUGUI title_tintB;
    [SerializeField] private Slider slider_tintB_metallic;
    [SerializeField] private Slider slider_tintB_smoothness;
    [SerializeField] private Slider slider_tintB_flakeInfluence;
    [SerializeField] private Slider slider_tintB_darkenInfluence;
    [SerializeField] private TextMeshProUGUI number_tintB_metallic;
    [SerializeField] private TextMeshProUGUI number_tintB_smoothness;
    [SerializeField] TextMeshProUGUI number_tintB_flakeInfluence;
    [SerializeField] TextMeshProUGUI number_tintB_darkenInfluence;

    [Header("Tint C")]
    [SerializeField] private TextMeshProUGUI title_tintC;
    [SerializeField] private Slider slider_tintC_metallic;
    [SerializeField] private Slider slider_tintC_smoothness;
    [SerializeField] private Slider slider_tintC_flakeInfluence;
    [SerializeField] private Slider slider_tintC_darkenInfluence;
    [SerializeField] TextMeshProUGUI number_tintC_metallic;
    [SerializeField] TextMeshProUGUI number_tintC_smoothness;
    [SerializeField] TextMeshProUGUI number_tintC_flakeInfluence;
    [SerializeField] TextMeshProUGUI number_tintC_darkenInfluence;

    [Space]
    [SerializeField] private TextMeshProUGUI title_emission;
    [SerializeField] private TextMeshProUGUI title_decal;
    #endregion

    #region pribate Fields
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

    private const string _f0 = "F0";
    #endregion

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
        slider_baseColor_flakeInfluence.value = 0;
        slider_baseColor_darkenInfluence.value = 0;
        number_baseColor_metallic.text = "0";
        number_baseColor_smoothness.text = "0";
        number_baseColor_flakeInfluence.text = "0";
        number_baseColor_darkenInfluence.text = "0";

        slider_tintA_metallic.value = 0;
        slider_tintA_smoothness.value = 0;
        slider_tintA_flakeInfluence.value = 0;
        slider_tintA_darkenInfluence.value = 0;
        number_tintA_metallic.text = "0";
        number_tintA_smoothness.text = "0";
        number_tintA_flakeInfluence.text = "0";
        number_tintA_darkenInfluence.text = "0";

        slider_tintB_metallic.value = 0;
        slider_tintB_smoothness.value = 0;
        slider_tintB_flakeInfluence.value = 0;
        slider_tintB_darkenInfluence.value = 0;
        number_tintB_metallic.text = "0";
        number_tintB_smoothness.text = "0";
        number_tintB_flakeInfluence.text = "0";
        number_tintB_darkenInfluence.text = "0";

        slider_tintC_metallic.value = 0;
        slider_tintC_smoothness.value = 0;
        slider_tintC_flakeInfluence.value = 0;
        slider_tintC_darkenInfluence.value = 0;
        number_tintC_metallic.text = "0";
        number_tintC_smoothness.text = "0";
        number_tintC_flakeInfluence.text = "0";
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

        currentDecal = -1;
        Decals_Next();

        SetColor_BodyParts(CarColorType.All);
        SetColors_Slider_Body();
        SetDecals_Body();

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
        SetColors_Sliders_BodyKit();
        SetDecals_BodyKit();
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
        SetColors_Sliders_Wheels();
    }
    #endregion

    #region Engine
    private async UniTaskVoid Engine_Next()
    {
        currentRootReferences.renderer_engine =
        (await Extensions.AsyncInstantiate(partsList.cars[currentPart_body].engine.part, currentRootReferences.root_engine)).GetComponentInChildren<Renderer>();

        currentRootReferences.material_engine = currentRootReferences.renderer_engine.material;
        SetColor_EnginePart(CarColorType.All);
        SetColors_Slider_Engine();
        SetDecals_Engine();
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
        SetColors_Sliders_SteeringWheel();
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

    public void Colors_Base_Random()
    {
        currentColor_base = Random.Range(0, colorsList.colors.Length);
        slider_baseColor_metallic.value = Random.Range(0f, 1f);
        slider_baseColor_smoothness.value = Random.Range(0f, 1f);
        slider_baseColor_flakeInfluence.value = Random.Range(0f, 1f);
        slider_baseColor_darkenInfluence.value = Random.Range(0f, 1f);

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

    public void ColorsSlider_Base_Metallic(float value)
    {
        SetColors_Slider(CarColorType.Base, CarColorSlider.Metallic, value);
        number_baseColor_metallic.text = (value * 100).ToString(_f0, System.Globalization.CultureInfo.InvariantCulture);
    }

    public void ColorsSlider_Base_Smoothness(float value)
    {
        SetColors_Slider(CarColorType.Base, CarColorSlider.Smoothness, value);
        number_baseColor_smoothness.text = (value * 100).ToString(_f0, System.Globalization.CultureInfo.InvariantCulture);
    }

    public void ColorsSlider_Base_FlakeInfluence(float value)
    {
        SetColors_Slider(CarColorType.Base, CarColorSlider.FlakeInfluence, value);
        number_baseColor_flakeInfluence.text = (value * 100).ToString(_f0, System.Globalization.CultureInfo.InvariantCulture);
    }

    public void ColorsSlider_Base_DarkenInfluence(float value)
    {
        SetColors_Slider(CarColorType.Base, CarColorSlider.DarkenInfluence, value);
        number_baseColor_darkenInfluence.text = (value * 100).ToString(_f0, System.Globalization.CultureInfo.InvariantCulture);
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

    public void Colors_TintA_Random()
    {
        currentColor_tintA = Random.Range(0, colorsList.colors.Length);
        slider_tintA_metallic.value = Random.Range(0f, 1f);
        slider_tintA_smoothness.value = Random.Range(0f, 1f);
        slider_tintA_flakeInfluence.value = Random.Range(0f, 1f);
        slider_tintA_darkenInfluence.value = Random.Range(0f, 1f);

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

    public void ColorsSlider_TintA_Metallic(float value)
    {
        SetColors_Slider(CarColorType.TintA, CarColorSlider.Metallic, value);
        number_tintA_metallic.text = (value * 100).ToString(_f0, System.Globalization.CultureInfo.InvariantCulture);
    }

    public void ColorsSlider_TintA_Smoothness(float value)
    {
        SetColors_Slider(CarColorType.TintA, CarColorSlider.Smoothness, value);
        number_tintA_smoothness.text = (value * 100).ToString(_f0, System.Globalization.CultureInfo.InvariantCulture);
    }

    public void ColorsSlider_TintA_FlakeInfluence(float value)
    {
        SetColors_Slider(CarColorType.TintA, CarColorSlider.FlakeInfluence, value);
        number_tintA_flakeInfluence.text = (value * 100).ToString(_f0, System.Globalization.CultureInfo.InvariantCulture);
    }

    public void ColorsSlider_TintA_DarkenInfluence(float value)
    {
        SetColors_Slider(CarColorType.TintA, CarColorSlider.DarkenInfluence, value);
        number_tintA_darkenInfluence.text = (value * 100).ToString(_f0, System.Globalization.CultureInfo.InvariantCulture);
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

    public void Colors_TintB_Random()
    {
        currentColor_tintB = Random.Range(0, colorsList.colors.Length);
        slider_tintB_metallic.value = Random.Range(0f, 1f);
        slider_tintB_smoothness.value = Random.Range(0f, 1f);
        slider_tintB_flakeInfluence.value = Random.Range(0f, 1f);
        slider_tintB_darkenInfluence.value = Random.Range(0f, 1f);

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

    public void ColorsSlider_TintB_Metallic(float value)
    {
        SetColors_Slider(CarColorType.TintB, CarColorSlider.Metallic, value);
        number_tintB_metallic.text = (value * 100).ToString(_f0, System.Globalization.CultureInfo.InvariantCulture);
    }

    public void ColorsSlider_TintB_Smoothness(float value)
    {
        SetColors_Slider(CarColorType.TintB, CarColorSlider.Smoothness, value);
        number_tintB_smoothness.text = (value * 100).ToString(_f0, System.Globalization.CultureInfo.InvariantCulture);
    }

    public void ColorsSlider_TintB_FlakeInfluence(float value)
    {
        SetColors_Slider(CarColorType.TintB, CarColorSlider.FlakeInfluence, value);
        number_tintB_flakeInfluence.text = (value * 100).ToString(_f0, System.Globalization.CultureInfo.InvariantCulture);
    }

    public void ColorsSlider_TintB_DarkenInfluence(float value)
    {
        SetColors_Slider(CarColorType.TintB, CarColorSlider.DarkenInfluence, value);
        number_tintB_darkenInfluence.text = (value * 100).ToString(_f0, System.Globalization.CultureInfo.InvariantCulture);
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

    public void Colors_TintC_Random()
    {
        currentColor_tintC = Random.Range(0, colorsList.colors.Length);
        slider_tintC_metallic.value = Random.Range(0f, 1f);
        slider_tintC_smoothness.value = Random.Range(0f, 1f);
        slider_tintC_flakeInfluence.value = Random.Range(0f, 1f);
        slider_tintC_darkenInfluence.value = Random.Range(0f, 1f);

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

    public void ColorsSlider_TintC_Metallic(float value)
    {
        SetColors_Slider(CarColorType.TintC, CarColorSlider.Metallic, value);
        number_tintC_metallic.text = (value * 100).ToString(_f0, System.Globalization.CultureInfo.InvariantCulture);
    }

    public void ColorsSlider_TintC_Smoothness(float value)
    {
        SetColors_Slider(CarColorType.TintC, CarColorSlider.Smoothness, value);
        number_tintC_smoothness.text = (value * 100).ToString(_f0, System.Globalization.CultureInfo.InvariantCulture);
    }

    public void ColorsSlider_TintC_FlakeInfluence(float value)
    {
        SetColors_Slider(CarColorType.TintC, CarColorSlider.FlakeInfluence, value);
        number_tintC_flakeInfluence.text = (value * 100).ToString(_f0, System.Globalization.CultureInfo.InvariantCulture);
    }

    public void ColorsSlider_TintC_DarkenInfluence(float value)
    {
        SetColors_Slider(CarColorType.TintC, CarColorSlider.DarkenInfluence, value);
        number_tintC_darkenInfluence.text = (value * 100).ToString(_f0, System.Globalization.CultureInfo.InvariantCulture);
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

    public void Colors_Emission_Random()
    {
        currentColor_emission = Random.Range(0, colorsList.colors.Length);

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

    public void Colors_All_Random()
    {
        Colors_Base_Random();
        Colors_TintA_Random();
        Colors_TintB_Random();
        Colors_TintC_Random();
        Colors_Emission_Random();
    }

    #region Set Parts Colors

    #region Material Colors
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

    #region Material Sliders
    private void SetColors_Slider(CarColorType colorType, CarColorSlider propertieType, float value)
    {
        if (currentRootReferences == null)
            return;

        SetColors_Slider_Body(colorType, propertieType, value);
        SetColors_Sliders_BodyKit(colorType, propertieType, value);
        SetColors_Slider_Engine(colorType, propertieType, value);
        SetColors_Sliders_SteeringWheel(colorType, propertieType, value);
        SetColors_Sliders_Wheels(colorType, propertieType, value);
    }

    private void SetColors_Slider_Body()
    {
        SetColors_Slider_Body(CarColorType.Base, CarColorSlider.Metallic, slider_baseColor_metallic.value);
        SetColors_Slider_Body(CarColorType.Base, CarColorSlider.Smoothness, slider_baseColor_smoothness.value);
        SetColors_Slider_Body(CarColorType.Base, CarColorSlider.FlakeInfluence, slider_baseColor_flakeInfluence.value);
        SetColors_Slider_Body(CarColorType.Base, CarColorSlider.DarkenInfluence, slider_baseColor_darkenInfluence.value);

        SetColors_Slider_Body(CarColorType.TintA, CarColorSlider.Metallic, slider_tintA_metallic.value);
        SetColors_Slider_Body(CarColorType.TintA, CarColorSlider.Smoothness, slider_tintA_smoothness.value);
        SetColors_Slider_Body(CarColorType.TintA, CarColorSlider.FlakeInfluence, slider_tintA_flakeInfluence.value);
        SetColors_Slider_Body(CarColorType.TintA, CarColorSlider.DarkenInfluence, slider_tintA_darkenInfluence.value);

        SetColors_Slider_Body(CarColorType.TintB, CarColorSlider.Metallic, slider_tintB_metallic.value);
        SetColors_Slider_Body(CarColorType.TintB, CarColorSlider.Smoothness, slider_tintB_smoothness.value);
        SetColors_Slider_Body(CarColorType.TintB, CarColorSlider.FlakeInfluence, slider_tintB_flakeInfluence.value);
        SetColors_Slider_Body(CarColorType.TintB, CarColorSlider.DarkenInfluence, slider_tintB_darkenInfluence.value);

        SetColors_Slider_Body(CarColorType.TintC, CarColorSlider.Metallic, slider_tintC_metallic.value);
        SetColors_Slider_Body(CarColorType.TintC, CarColorSlider.Smoothness, slider_tintC_smoothness.value);
        SetColors_Slider_Body(CarColorType.TintC, CarColorSlider.FlakeInfluence, slider_tintC_flakeInfluence.value);
        SetColors_Slider_Body(CarColorType.TintC, CarColorSlider.DarkenInfluence, slider_tintC_darkenInfluence.value);
    }
    private void SetColors_Slider_Body(CarColorType colorType, CarColorSlider propertieType, float value)
    {
        if (!CarColorsProperties.propertyMap.TryGetValue((colorType, propertieType), out int propertie))
            return;

        if (currentRootReferences.material_body)
            currentRootReferences.material_body.SetFloat(propertie, value);
        if (currentRootReferences.material_chassis)
            currentRootReferences.material_chassis.SetFloat(propertie, value);
        if (currentRootReferences.material_dash)
            currentRootReferences.material_dash.SetFloat(propertie, value);
        if (currentRootReferences.material_emissive)
            currentRootReferences.material_emissive.SetFloat(propertie, value);
        if (currentRootReferences.material_glass)
            currentRootReferences.material_glass.SetFloat(propertie, value);
        if (currentRootReferences.material_interior)
            currentRootReferences.material_interior.SetFloat(propertie, value);
    }

    private void SetColors_Sliders_BodyKit()
    {
        SetColors_Sliders_BodyKit(CarColorType.Base, CarColorSlider.Metallic, slider_baseColor_metallic.value);
        SetColors_Sliders_BodyKit(CarColorType.Base, CarColorSlider.Smoothness, slider_baseColor_smoothness.value);
        SetColors_Sliders_BodyKit(CarColorType.Base, CarColorSlider.FlakeInfluence, slider_baseColor_flakeInfluence.value);
        SetColors_Sliders_BodyKit(CarColorType.Base, CarColorSlider.DarkenInfluence, slider_baseColor_darkenInfluence.value);

        SetColors_Sliders_BodyKit(CarColorType.TintA, CarColorSlider.Metallic, slider_tintA_metallic.value);
        SetColors_Sliders_BodyKit(CarColorType.TintA, CarColorSlider.Smoothness, slider_tintA_smoothness.value);
        SetColors_Sliders_BodyKit(CarColorType.TintA, CarColorSlider.FlakeInfluence, slider_tintA_flakeInfluence.value);
        SetColors_Sliders_BodyKit(CarColorType.TintA, CarColorSlider.DarkenInfluence, slider_tintA_darkenInfluence.value);

        SetColors_Sliders_BodyKit(CarColorType.TintB, CarColorSlider.Metallic, slider_tintB_metallic.value);
        SetColors_Sliders_BodyKit(CarColorType.TintB, CarColorSlider.Smoothness, slider_tintB_smoothness.value);
        SetColors_Sliders_BodyKit(CarColorType.TintB, CarColorSlider.FlakeInfluence, slider_tintB_flakeInfluence.value);
        SetColors_Sliders_BodyKit(CarColorType.TintB, CarColorSlider.DarkenInfluence, slider_tintB_darkenInfluence.value);

        SetColors_Sliders_BodyKit(CarColorType.TintC, CarColorSlider.Metallic, slider_tintC_metallic.value);
        SetColors_Sliders_BodyKit(CarColorType.TintC, CarColorSlider.Smoothness, slider_tintC_smoothness.value);
        SetColors_Sliders_BodyKit(CarColorType.TintC, CarColorSlider.FlakeInfluence, slider_tintC_flakeInfluence.value);
        SetColors_Sliders_BodyKit(CarColorType.TintC, CarColorSlider.DarkenInfluence, slider_tintC_darkenInfluence.value);
    }
    private void SetColors_Sliders_BodyKit(CarColorType colorType, CarColorSlider propertieType, float value)
    {
        if (!CarColorsProperties.propertyMap.TryGetValue((colorType, propertieType), out int propertie))
            return;

        if (currentRootReferences.material_bodyKit)
            currentRootReferences.material_bodyKit.SetFloat(propertie, value);
    }

    private void SetColors_Slider_Engine()
    {
        SetColors_Slider_Engine(CarColorType.Base, CarColorSlider.Metallic, slider_baseColor_metallic.value);
        SetColors_Slider_Engine(CarColorType.Base, CarColorSlider.Smoothness, slider_baseColor_smoothness.value);
        SetColors_Slider_Engine(CarColorType.Base, CarColorSlider.FlakeInfluence, slider_baseColor_flakeInfluence.value);
        SetColors_Slider_Engine(CarColorType.Base, CarColorSlider.DarkenInfluence, slider_baseColor_darkenInfluence.value);

        SetColors_Slider_Engine(CarColorType.TintA, CarColorSlider.Metallic, slider_tintA_metallic.value);
        SetColors_Slider_Engine(CarColorType.TintA, CarColorSlider.Smoothness, slider_tintA_smoothness.value);
        SetColors_Slider_Engine(CarColorType.TintA, CarColorSlider.FlakeInfluence, slider_tintA_flakeInfluence.value);
        SetColors_Slider_Engine(CarColorType.TintA, CarColorSlider.DarkenInfluence, slider_tintA_darkenInfluence.value);

        SetColors_Slider_Engine(CarColorType.TintB, CarColorSlider.Metallic, slider_tintB_metallic.value);
        SetColors_Slider_Engine(CarColorType.TintB, CarColorSlider.Smoothness, slider_tintB_smoothness.value);
        SetColors_Slider_Engine(CarColorType.TintB, CarColorSlider.FlakeInfluence, slider_tintB_flakeInfluence.value);
        SetColors_Slider_Engine(CarColorType.TintB, CarColorSlider.DarkenInfluence, slider_tintB_darkenInfluence.value);

        SetColors_Slider_Engine(CarColorType.TintC, CarColorSlider.Metallic, slider_tintC_metallic.value);
        SetColors_Slider_Engine(CarColorType.TintC, CarColorSlider.Smoothness, slider_tintC_smoothness.value);
        SetColors_Slider_Engine(CarColorType.TintC, CarColorSlider.FlakeInfluence, slider_tintC_flakeInfluence.value);
        SetColors_Slider_Engine(CarColorType.TintC, CarColorSlider.DarkenInfluence, slider_tintC_darkenInfluence.value);
    }
    private void SetColors_Slider_Engine(CarColorType colorType, CarColorSlider propertieType, float value)
    {
        if (!CarColorsProperties.propertyMap.TryGetValue((colorType, propertieType), out int propertie))
            return;

        if (currentRootReferences.material_engine)
            currentRootReferences.material_engine.SetFloat(propertie, value);
    }

    private void SetColors_Sliders_SteeringWheel()
    {
        SetColors_Sliders_SteeringWheel(CarColorType.Base, CarColorSlider.Metallic, slider_baseColor_metallic.value);
        SetColors_Sliders_SteeringWheel(CarColorType.Base, CarColorSlider.Smoothness, slider_baseColor_smoothness.value);
        SetColors_Sliders_SteeringWheel(CarColorType.Base, CarColorSlider.FlakeInfluence, slider_baseColor_flakeInfluence.value);
        SetColors_Sliders_SteeringWheel(CarColorType.Base, CarColorSlider.DarkenInfluence, slider_baseColor_darkenInfluence.value);

        SetColors_Sliders_SteeringWheel(CarColorType.TintA, CarColorSlider.Metallic, slider_tintA_metallic.value);
        SetColors_Sliders_SteeringWheel(CarColorType.TintA, CarColorSlider.Smoothness, slider_tintA_smoothness.value);
        SetColors_Sliders_SteeringWheel(CarColorType.TintA, CarColorSlider.FlakeInfluence, slider_tintA_flakeInfluence.value);
        SetColors_Sliders_SteeringWheel(CarColorType.TintA, CarColorSlider.DarkenInfluence, slider_tintA_darkenInfluence.value);

        SetColors_Sliders_SteeringWheel(CarColorType.TintB, CarColorSlider.Metallic, slider_tintB_metallic.value);
        SetColors_Sliders_SteeringWheel(CarColorType.TintB, CarColorSlider.Smoothness, slider_tintB_smoothness.value);
        SetColors_Sliders_SteeringWheel(CarColorType.TintB, CarColorSlider.FlakeInfluence, slider_tintB_flakeInfluence.value);
        SetColors_Sliders_SteeringWheel(CarColorType.TintB, CarColorSlider.DarkenInfluence, slider_tintB_darkenInfluence.value);

        SetColors_Sliders_SteeringWheel(CarColorType.TintC, CarColorSlider.Metallic, slider_tintC_metallic.value);
        SetColors_Sliders_SteeringWheel(CarColorType.TintC, CarColorSlider.Smoothness, slider_tintC_smoothness.value);
        SetColors_Sliders_SteeringWheel(CarColorType.TintC, CarColorSlider.FlakeInfluence, slider_tintC_flakeInfluence.value);
        SetColors_Sliders_SteeringWheel(CarColorType.TintC, CarColorSlider.DarkenInfluence, slider_tintC_darkenInfluence.value);
    }
    private void SetColors_Sliders_SteeringWheel(CarColorType colorType, CarColorSlider propertieType, float value)
    {
        if (!CarColorsProperties.propertyMap.TryGetValue((colorType, propertieType), out int propertie))
            return;

        if (currentRootReferences.material_steeringWheel)
            currentRootReferences.material_steeringWheel.SetFloat(propertie, value);
    }

    private void SetColors_Sliders_Wheels()
    {
        SetColors_Sliders_Wheels(CarColorType.Base, CarColorSlider.Metallic, slider_baseColor_metallic.value);
        SetColors_Sliders_Wheels(CarColorType.Base, CarColorSlider.Smoothness, slider_baseColor_smoothness.value);
        SetColors_Sliders_Wheels(CarColorType.Base, CarColorSlider.FlakeInfluence, slider_baseColor_flakeInfluence.value);
        SetColors_Sliders_Wheels(CarColorType.Base, CarColorSlider.DarkenInfluence, slider_baseColor_darkenInfluence.value);

        SetColors_Sliders_Wheels(CarColorType.TintA, CarColorSlider.Metallic, slider_tintA_metallic.value);
        SetColors_Sliders_Wheels(CarColorType.TintA, CarColorSlider.Smoothness, slider_tintA_smoothness.value);
        SetColors_Sliders_Wheels(CarColorType.TintA, CarColorSlider.FlakeInfluence, slider_tintA_flakeInfluence.value);
        SetColors_Sliders_Wheels(CarColorType.TintA, CarColorSlider.DarkenInfluence, slider_tintA_darkenInfluence.value);

        SetColors_Sliders_Wheels(CarColorType.TintB, CarColorSlider.Metallic, slider_tintB_metallic.value);
        SetColors_Sliders_Wheels(CarColorType.TintB, CarColorSlider.Smoothness, slider_tintB_smoothness.value);
        SetColors_Sliders_Wheels(CarColorType.TintB, CarColorSlider.FlakeInfluence, slider_tintB_flakeInfluence.value);
        SetColors_Sliders_Wheels(CarColorType.TintB, CarColorSlider.DarkenInfluence, slider_tintB_darkenInfluence.value);

        SetColors_Sliders_Wheels(CarColorType.TintC, CarColorSlider.Metallic, slider_tintC_metallic.value);
        SetColors_Sliders_Wheels(CarColorType.TintC, CarColorSlider.Smoothness, slider_tintC_smoothness.value);
        SetColors_Sliders_Wheels(CarColorType.TintC, CarColorSlider.FlakeInfluence, slider_tintC_flakeInfluence.value);
        SetColors_Sliders_Wheels(CarColorType.TintC, CarColorSlider.DarkenInfluence, slider_tintC_darkenInfluence.value);
    }
    private void SetColors_Sliders_Wheels(CarColorType colorType, CarColorSlider propertieType, float value)
    {
        if (!CarColorsProperties.propertyMap.TryGetValue((colorType, propertieType), out int propertie))
            return;

        if (currentRootReferences.material_wheels)
            currentRootReferences.material_wheels.SetFloat(propertie, value);
    }
    #endregion

    #endregion

    #endregion

    #region Decals
    public void Decals_Back()
    {
        currentDecal--;

        if (currentDecal < 0)
        {
            currentDecal = partsList.cars[currentPart_body].decals.Length - 1;
        }

        SetDecals();
    }

    public void Decals_Next()
    {
        currentDecal++;

        if (currentDecal > partsList.cars[currentPart_body].decals.Length - 1)
        {
            currentDecal = 0;
        }

        SetDecals();
    }

    private void SetDecals()
    {
        title_decal.text = partsList.cars[currentPart_body].decals[currentDecal].name;

        SetDecals_Body();
        SetDecals_BodyKit();
        SetDecals_Engine();
    }

    private void SetDecals_Body()
    {
        if (currentRootReferences.material_body != null)
            currentRootReferences.material_body.SetTexture(CarColorsProperties.DecalTint, partsList.cars[currentPart_body].decals[currentDecal].body);
        if (currentRootReferences.material_chassis != null)
            currentRootReferences.material_chassis.SetTexture(CarColorsProperties.DecalTint, partsList.cars[currentPart_body].decals[currentDecal].chasis);
        if (currentRootReferences.material_dash != null)
            currentRootReferences.material_dash.SetTexture(CarColorsProperties.DecalTint, partsList.cars[currentPart_body].decals[currentDecal].dash);
        if (currentRootReferences.material_emissive != null)
            currentRootReferences.material_emissive.SetTexture(CarColorsProperties.DecalTint, partsList.cars[currentPart_body].decals[currentDecal].emissive);
        if (currentRootReferences.material_glass != null)
            currentRootReferences.material_glass.SetTexture(CarColorsProperties.DecalTint, partsList.cars[currentPart_body].decals[currentDecal].glass);
        if (currentRootReferences.material_interior != null)
            currentRootReferences.material_interior.SetTexture(CarColorsProperties.DecalTint, partsList.cars[currentPart_body].decals[currentDecal].interior);
    }

    private void SetDecals_BodyKit()
    {
        if (currentRootReferences.material_bodyKit != null)
            currentRootReferences.material_bodyKit.SetTexture(CarColorsProperties.DecalTint, partsList.cars[currentPart_body].decals[currentDecal].bodyKit);
    }

    private void SetDecals_Engine()
    {
        if (currentRootReferences.material_engine != null)
            currentRootReferences.material_engine.SetTexture(CarColorsProperties.DecalTint, partsList.cars[currentPart_body].decals[currentDecal].engine);
    }
    #endregion
}