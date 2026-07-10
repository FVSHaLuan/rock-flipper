using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSizeByZPosition : ParticleManipulator
{
    [SerializeField]
    private float farZ = 1;
    [SerializeField]
    private float nearZ = -1;

    [Space]
    [SerializeField]
    private float farScale = 0.1f;
    [SerializeField]
    private float nearScale = 1;

    protected override ParticleSystem.Particle Manipulate(ParticleSystem.Particle particle)
    {
        ///
        var cd = GetCustomData(ParticleSystemCustomData.Custom1);

        ///
        if (cd.w <= 0)
        {
            cd.w = 1;
            cd.x = particle.startSize;
            SetCustomData(cd, ParticleSystemCustomData.Custom1);
        }

        ///
        var currentClampedZ = Mathf.Clamp(particle.position.z, nearZ, farZ);
        var scale = Mathf.Lerp(farScale, nearScale, Mathf.InverseLerp(nearZ, farZ, currentClampedZ));

        ///
        particle.startSize = cd.x * scale;

        ///
        return particle;
    }
}
