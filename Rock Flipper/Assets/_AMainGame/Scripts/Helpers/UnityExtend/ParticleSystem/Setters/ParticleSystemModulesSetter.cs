using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleSystemModulesSetter : MonoBehaviour
{
    private new ParticleSystem particleSystem;

    private ParticleSystem ParticleSystem
    {
        get
        {
            ///
            if (particleSystem == null)
            {
                particleSystem = GetComponent<ParticleSystem>();
            }

            ///
            return particleSystem;
        }
    }

    public bool ModuleSizeOverLifetime
    {
        get => ParticleSystem.sizeOverLifetime.enabled;
        set { var m = ParticleSystem.sizeOverLifetime; m.enabled = value; }
    }

    public bool ModuleVelocityOverLifetime
    {
        get => ParticleSystem.velocityOverLifetime.enabled;
        set { var m = ParticleSystem.velocityOverLifetime; m.enabled = value; }
    }

    public bool ModuleExternalForces
    {
        get => ParticleSystem.externalForces.enabled;
        set { var m = ParticleSystem.externalForces; m.enabled = value; }
    }

    public float SimulationSpeed
    {
        get => ParticleSystem.main.simulationSpeed;
        set
        {
            var m = particleSystem.main;
            m.simulationSpeed = value;
        }
    }
}
