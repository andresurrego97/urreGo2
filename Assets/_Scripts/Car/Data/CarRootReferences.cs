using System.Collections.Generic;
using UnityEngine;

public class CarRootReferences : MonoBehaviour
{
    public Animator anim;
    public Transform cameraRoot;

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
    public ParticleSystem[] particles_exhaust;
    public Material particles_exhaustMaterial;
    public ParticleSystem particles_sparks;

    [Header("Removable parts")]
    public List<CarDetachedPart> removableParts;

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

    private void Awake()
    {
        for (int i = 0; i < particles_exhaust.Length; i++)
        {
            particles_exhaust[i].TryGetComponent(out ParticleSystemRenderer render);

            if (i == 0)
            {
                particles_exhaustMaterial = Instantiate(render.material);
                render.material = particles_exhaustMaterial;
            }
            else
            {
                render.material = particles_exhaustMaterial;
            }
        }
    }

    //private void Awake()
    //{
    //    suspensionCenters = new Vector3[root_suspension.Length];
    //    for (int i = 0; i < root_suspension.Length; i++)
    //    {
    //        suspensionCenters[i] = root_suspension[i].localPosition;
    //    }
    //}
}