using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleSystemEmissionRatePerShapeArea : MonoBehaviour
{
    [SerializeField]
    private float emissionRatePerVolumeUnit = 1;

    private new ParticleSystem particleSystem;

    [ContextMenu("UpdateEmissionRate")]
    public void UpdateEmissionRate()
    {
        ///
        if (particleSystem == null)
        {
            particleSystem = GetComponent<ParticleSystem>();
        }

        ///
        var shapeScale = particleSystem.shape.scale;
        var volume = shapeScale.x * shapeScale.y * shapeScale.z;

        ///
        if (particleSystem.main.scalingMode == ParticleSystemScalingMode.Shape)
        {
            volume *= transform.lossyScale.x * transform.lossyScale.y * transform.lossyScale.z;
        }

        ///
        var emission = particleSystem.emission;
        var rateOverTime = emission.rateOverTime;
        rateOverTime.constant = volume * emissionRatePerVolumeUnit;
        emission.rateOverTime = rateOverTime;
    }

#if UNITY_EDITOR
    [ContextMenu("Editor_CalculateEmissionRatePerVolumeUnit")]
    private void Editor_CalculateEmissionRatePerVolumeUnit()
    {
        ///
        UnityEditor.Undo.RecordObject(this, "Editor_CalculateEmissionRatePerVolumeUnit");

        ///
        particleSystem = GetComponent<ParticleSystem>();

        ///
        var shapeScale = particleSystem.shape.scale;
        var volume = shapeScale.x * shapeScale.y * shapeScale.z;
        emissionRatePerVolumeUnit = particleSystem.emission.rateOverTime.constant / volume;
    }
#endif
}
