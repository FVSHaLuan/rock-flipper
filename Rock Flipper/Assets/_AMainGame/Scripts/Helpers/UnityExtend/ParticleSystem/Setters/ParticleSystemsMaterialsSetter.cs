using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemsMaterialsSetter : MonoBehaviour
{
    [SerializeField]
    private List<ParticleSystemRenderer> particleSystemRenderers;

    [Space]
    [SerializeField]
    private Material mainMaterial;
    [SerializeField]
    private Material trailMaterial;

    [ContextMenu("Set")]
    public void Set()
    {
        foreach (var particleSystemRenderer in particleSystemRenderers)
        {
            particleSystemRenderer.material = mainMaterial;
            particleSystemRenderer.trailMaterial = trailMaterial; 
        }
    }
}
