using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleSystemRotateWithGameObject : ParticleManipulator
{
    [SerializeField]
    private bool isLocalRotation = false;

    private float currentAngle;

    protected override void OnStartManipulatingParticles()
    {
        ///
        base.OnStartManipulatingParticles();

        ///
        currentAngle = isLocalRotation ? -transform.localEulerAngles.z : -transform.eulerAngles.z;
    }

    protected override ParticleSystem.Particle Manipulate(ParticleSystem.Particle particle)
    {
        ///
        particle.rotation = currentAngle;

        ///
        return particle;
    }
}
