using UnityEngine;

public class CarRootReferences : MonoBehaviour
{
    [Header("Roots")]
    public Transform root_bodyKit;
    public Transform root_engine;
    public Transform root_steeringWheel;
    public Transform[] root_brakes;
    public Transform[] root_frontWheels;
    public Transform[] root_backWheels;

    [Header("Renderers")]
    public SkinnedMeshRenderer renderer_body;
    public SkinnedMeshRenderer renderer_chassis;
    public SkinnedMeshRenderer renderer_dash;
    public SkinnedMeshRenderer renderer_emissive;
    public SkinnedMeshRenderer renderer_glass;
    public SkinnedMeshRenderer renderer_interior;

    [Space]
    public Renderer renderer_bodyKit;
    public Renderer renderer_engine;
    public Renderer renderer_steeringWheel;
    public Renderer[] renderer_brakes;
    public Renderer[] renderer_frontWheels;
    public Renderer[] renderer_backWheels;

    [Space]
    public Material material_body;
    public Material material_chassis;
    public Material material_dash;
    public Material material_emissive;
    public Material material_glass;
    public Material material_interior;

    public Material material_bodyKit;
    public Material material_engine;
    public Material material_steeringWheel;
    public Material material_wheels;

    [Space]
    public Animator anim;
}