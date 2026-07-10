using FH.Core;
using FH.Core.HelperComponent;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleAttractorLerp : ParticleManipulator
{
    [Header("ParticleAttractor")]
    [SerializeField]
    private PositionProvider target;
    [SerializeField]
    private bool killParticleWhenReachedTarget;

    protected override ParticleSystem.Particle Manipulate(ParticleSystem.Particle particle)
    {
        ///
        var targetPos = target.Position;

        ///
        var customData = GetCustomData(ParticleSystemCustomData.Custom1);

        ///
        Vector3 startPos = particle.position;

        ///
        if (customData.w == 0)
        {
            ///
            customData.Set(startPos.x, startPos.y, startPos.z, 1);

            ///
            SetCustomData(customData, ParticleSystemCustomData.Custom1);
        }
        else
        {
            startPos = (Vector3)customData;
        }

        ///
        particle.position = Vector3.Lerp(startPos, targetPos, particle.CurrentProgress());

        ///
        if (killParticleWhenReachedTarget)
        {
            if (Mathf.Approximately(Vector3.SqrMagnitude(particle.position - targetPos), 0))
            {
                particle.remainingLifetime = 0;
            }
        }

        ///
        return particle;
    }
}
