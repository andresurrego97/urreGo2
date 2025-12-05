using Cysharp.Threading.Tasks;
using System;
using System.Linq;
using TMPro;
using UnityEngine;

public class CarPartsChanger : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CarPartsList list;
    [SerializeField] private Transform root;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI title_body;
    [SerializeField] private TextMeshProUGUI title_bodyKit;
    [SerializeField] private TextMeshProUGUI title_steeringWheel;
    [SerializeField] private TextMeshProUGUI title_wheels;

    private int currentBody = -1;
    private bool currentBodyKit = false;
    private int currentSteeringWheel = -1;
    private int currentWheels = -1;

    private CarRootReferences currentRootReferences;

    #region Body
    public void Body_Back()
    {
        currentBody--;

        if (currentBody < 0)
        {
            currentBody = list.cars.Length - 1;
        }

        Instantiate_Body().Forget();
    }

    public void Body_Next()
    {
        currentBody++;

        if (currentBody > list.cars.Length -1)
        {
            currentBody = 0;
        }

        Instantiate_Body().Forget();
    }

    private async UniTaskVoid Instantiate_Body()
    {
        title_body.text = list.cars[currentBody].name;

        for (int i = 0; i < root.childCount; i++)
        {
            Destroy(root.GetChild(i).gameObject);
        }

        currentRootReferences =
            (await Extensions.AsyncInstantiate(list.cars[currentBody].body.part, root)).GetComponent<CarRootReferences>();

        currentBodyKit = false;
        BodyKit_Next();

        currentSteeringWheel = -1;
        currentSteeringWheel = Array.IndexOf(list.steeringWheels, list.cars[currentBody].steeringWheel) - 1;
        SteeringWheel_Next();

        currentWheels = -1;
        currentWheels = Array.IndexOf(list.comboWheels, list.cars[currentBody].wheel) - 1;
        Wheels_Next();
        // put default wheels for the car
        // default body kit
        // default stearing wheel
        // default combo wheels
    }
    #endregion

    #region Body Kit
    public void BodyKit_Next()
    {
        currentBodyKit = !currentBodyKit;

        if (list.cars[currentBody].bodyKit != null)
        {
            if (currentBodyKit)
            {
                title_bodyKit.text = list.cars[currentBody].bodyKit.name;
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
            (await Extensions.AsyncInstantiate(list.cars[currentBody].bodyKit.part, currentRootReferences.root_bodyKit)).GetComponentInChildren<Renderer>();
    }
    #endregion

    #region Steering Wheel
    public void SteeringWheel_Back()
    {
        currentSteeringWheel--;

        if (currentSteeringWheel < 0)
        {
            currentSteeringWheel = list.cars.Length - 1;
        }

        Instantiate_SteeringWheel().Forget();
    }

    public void SteeringWheel_Next()
    {
        currentSteeringWheel++;

        if (currentSteeringWheel > list.cars.Length - 1)
        {
            currentSteeringWheel = 0;
        }

        Instantiate_SteeringWheel().Forget();
    }

    private async UniTaskVoid Instantiate_SteeringWheel()
    {
        title_steeringWheel.text = list.steeringWheels[currentSteeringWheel].name;

        if (currentRootReferences.root_steeringWheel.childCount == 1)
        {
            Destroy(currentRootReferences.root_steeringWheel.GetChild(0).gameObject);
        }

        currentRootReferences.renderer_steeringWheel =
            (await Extensions.AsyncInstantiate(list.steeringWheels[currentSteeringWheel].part, currentRootReferences.root_steeringWheel)).GetComponentInChildren<Renderer>();
    }
    #endregion

    #region Wheels
    public void Wheels_Back()
    {
        currentWheels--;

        if (currentWheels < 0)
        {
            currentWheels = list.cars.Length - 1;
        }

        Instantiate_Wheels().Forget();
    }

    public void Wheels_Next()
    {
        currentWheels++;

        if (currentWheels > list.cars.Length - 1)
        {
            currentWheels = 0;
        }

        Instantiate_Wheels().Forget();
    }

    private async UniTaskVoid Instantiate_Wheels()
    {
        if (currentWheels != -1)
        {
            title_wheels.text = list.comboWheels[currentWheels].name;
        }
        else
        {
            title_wheels.text = "Default";
            return;
        }

        // brakes
        for (int i = 0; i < currentRootReferences.root_brakes.Length; i++)
        {
            if (currentRootReferences.root_brakes[i].childCount == 1)
            {
                Destroy(currentRootReferences.root_brakes[i].GetChild(0).gameObject);
            }
        }

        if (list.comboWheels[currentWheels].thirdPart != null)
        {
            currentRootReferences.renderer_brakes = new MeshRenderer[currentRootReferences.root_brakes.Length];
            for (int i = 0; i < currentRootReferences.root_brakes.Length; i++)
            {
                currentRootReferences.renderer_brakes[i] =
                    (await Extensions.AsyncInstantiate(list.comboWheels[currentWheels].thirdPart.part, currentRootReferences.root_brakes[i])).GetComponentInChildren<Renderer>();
            }
        }
        else
        {
            currentRootReferences.renderer_brakes = null;
        }

        // front
        for (int i = 0; i < currentRootReferences.root_frontWheels.Length; i++)
        {
            if (currentRootReferences.root_frontWheels[i].childCount == 1)
            {
                Destroy(currentRootReferences.root_frontWheels[i].GetChild(0).gameObject);
            }
        }

        if (list.comboWheels[currentWheels].firstPart != null)
        {
            currentRootReferences.renderer_frontWheels = new MeshRenderer[currentRootReferences.root_frontWheels.Length];
            for (int i = 0; i < currentRootReferences.root_frontWheels.Length; i++)
            {
                currentRootReferences.renderer_frontWheels[i] =
                    (await Extensions.AsyncInstantiate(list.comboWheels[currentWheels].firstPart.part, currentRootReferences.root_frontWheels[i])).GetComponentInChildren<Renderer>();
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

        if (list.comboWheels[currentWheels].secondPart != null)
        {
            currentRootReferences.renderer_backWheels = new MeshRenderer[currentRootReferences.root_backWheels.Length];
            for (int i = 0; i < currentRootReferences.root_backWheels.Length; i++)
            {
                currentRootReferences.renderer_backWheels[i] =
                    (await Extensions.AsyncInstantiate(list.comboWheels[currentWheels].secondPart.part, currentRootReferences.root_backWheels[i])).GetComponentInChildren<Renderer>();
            }
        }
        else if (list.comboWheels[currentWheels].firstPart != null)
        {
            currentRootReferences.renderer_backWheels = new MeshRenderer[currentRootReferences.root_backWheels.Length];
            for (int i = 0; i < currentRootReferences.root_backWheels.Length; i++)
            {
                currentRootReferences.renderer_backWheels[i] =
                    (await Extensions.AsyncInstantiate(list.comboWheels[currentWheels].firstPart.part, currentRootReferences.root_backWheels[i])).GetComponentInChildren<Renderer>();
            }
        }
        else
        {
            currentRootReferences.renderer_frontWheels = null;
        }
    }
    #endregion
}