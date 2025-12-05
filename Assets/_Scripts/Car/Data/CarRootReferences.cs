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
    public Renderer renderer_steeringWheel;
    public Renderer[] renderer_brakes;
    public Renderer[] renderer_frontWheels;
    public Renderer[] renderer_backWheels;

    [Space]
    public Animator anim;
}