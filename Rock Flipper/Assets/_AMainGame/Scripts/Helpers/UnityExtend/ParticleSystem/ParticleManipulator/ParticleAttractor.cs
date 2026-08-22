using FH.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleAttractor : ParticleManipulator
{
    [Header("ParticleAttractor")]
    [SerializeField]
    private PositionProvider target;
    [SerializeField]
    private float speed;
    [SerializeField]
    private bool killParticleWhenReachedTarget;

    protected override ParticleSystem.Particle Manipulate(ParticleSystem.Particle particle)
    {
        ///
        var targetPos = target.Position;

        ///
        particle.position = Vector3.MoveTowards(particle.position, targetPos, speed * Time.deltaTime);

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
