using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;

public class ParticleDieCallback : ParticleManipulator
{
    private int lastUpdatedFrame;

    protected virtual void OnParticleDied(ParticleSystem.Particle particle) { }

    protected void OnEnable()
    {
        updateType = UpdateMode.ManualUpdate;
    }

    protected void FixedUpdate()
    {
        if (lastUpdatedFrame != Time.frameCount
            && ParticleSystem.isPlaying)
        {
            ///
            lastUpdatedFrame = Time.frameCount;

            ///
            UpdateParticles();
        }
    }

    protected sealed override ParticleSystem.Particle Manipulate(ParticleSystem.Particle particle)
    {
        var deltaTime = ParticleSystem.main.useUnscaledTime ? Time.unscaledTime : Time.deltaTime;
        var effRemainingLifeTime = particle.remainingLifetime - deltaTime;

        ///
        if (effRemainingLifeTime <= 0)
        {
            OnParticleDied(particle);
        }

        ///
        return particle;
    }
}
