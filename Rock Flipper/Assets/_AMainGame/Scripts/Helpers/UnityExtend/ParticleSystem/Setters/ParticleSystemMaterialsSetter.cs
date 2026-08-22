using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemMaterialsSetter : MonoBehaviour
{
    [SerializeField]
    private ParticleSystemRenderer particleSystemRenderer;

    [Space]
    [SerializeField]
    private Material mainMaterial;
    [SerializeField]
    private Material trailMaterial;

    [ContextMenu("Set")]
    public void Set()
    {
        particleSystemRenderer.material = mainMaterial;
        particleSystemRenderer.trailMaterial = trailMaterial;
    }
}
