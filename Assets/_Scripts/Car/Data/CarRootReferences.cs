using UnityEngine;

public class CarRootReferences : MonoBehaviour
{
    public Animator anim;

    [Header("Roots")]
    public Transform root_bodyKit;
    public Transform root_engine;
    public Transform root_steeringWheel;
    public Transform[] root_frontSteering;
    public Transform[] root_brakes;
    public Transform[] root_frontWheels;
    public Transform[] root_backWheels;
    public Transform[] root_suspension;
    //[HideInInspector] public Vector3[] suspensionCenters;

    [Header("Renderers")]
    public SkinnedMeshRenderer renderer_body;
    public SkinnedMeshRenderer renderer_chassis;
    public SkinnedMeshRenderer renderer_dash;
    public SkinnedMeshRenderer renderer_emissive;
    public SkinnedMeshRenderer renderer_glass;
    public SkinnedMeshRenderer renderer_interior;

    [Header("Particles")]
    public ParticleSystem[] particles_drift;

    [Header("-- Runtime --")]
    public Renderer renderer_bodyKit;
    public Renderer renderer_engine;
    public Renderer renderer_steeringWheel;
    public Renderer[] renderer_brakes;
    public Renderer[] renderer_frontWheels;
    public Renderer[] renderer_backWheels;
    public float default_emissiveValue;

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

    //private void Awake()
    //{
    //    suspensionCenters = new Vector3[root_suspension.Length];
    //    for (int i = 0; i < root_suspension.Length; i++)
    //    {
    //        suspensionCenters[i] = root_suspension[i].localPosition;
    //    }
    //}
}