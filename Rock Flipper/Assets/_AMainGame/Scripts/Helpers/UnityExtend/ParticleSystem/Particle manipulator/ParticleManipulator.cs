using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public abstract class ParticleManipulator : MonoBehaviour
{
    private const int MaxParticle = 500;

    [Header("ParticleManipulator")]
    [SerializeField]
    protected UpdateMode updateType;

    private static ParticleSystem.Particle[] particles;
    private static List<Vector4> customData1 = new List<Vector4>();
    private static List<Vector4> customData2 = new List<Vector4>();

    private bool customDataDirtyFlag1;
    private bool customDataDirtyFlag2;

    protected ParticleSystem ParticleSystem { get; private set; }
    protected int ParticleCount { get; private set; }
    protected int CurrentParticleId { get; private set; }

    protected abstract ParticleSystem.Particle Manipulate(ParticleSystem.Particle particle);

    protected virtual void OnStartManipulatingParticles() { }
    protected virtual void OnFinishedManipulatingParticles() { }

    static ParticleManipulator()
    {
        particles = new ParticleSystem.Particle[MaxParticle];
    }

    protected virtual void Awake()
    {
        ParticleSystem = GetComponent<ParticleSystem>();
    }

    protected virtual void Update()
    {
        if (updateType == UpdateMode.Update
            && ParticleSystem.isPlaying)
        {
            UpdateParticles();
        }
    }

    protected virtual void LateUpdate()
    {
        if (updateType == UpdateMode.LateUpdate
            && ParticleSystem.isPlaying)
        {
            UpdateParticles();
        }
    }

    public void UpdateParticles()
    {
        ///
        OnStartManipulatingParticles();

        ///
        customDataDirtyFlag1 = customDataDirtyFlag2 = false;

        ///
        ParticleCount = ParticleSystem.GetParticles(particles);
        ParticleSystem.GetCustomParticleData(customData1, ParticleSystemCustomData.Custom1);
        ParticleSystem.GetCustomParticleData(customData2, ParticleSystemCustomData.Custom2);

        ///
        for (CurrentParticleId = 0; CurrentParticleId < ParticleCount; CurrentParticleId++)
        {
            var particle = particles[CurrentParticleId];
            particles[CurrentParticleId] = Manipulate(particle);
        }

        ///
        if (customDataDirtyFlag1)
        {
            ParticleSystem.SetCustomParticleData(customData1, ParticleSystemCustomData.Custom1);
        }
        if (customDataDirtyFlag2)
        {
            ParticleSystem.SetCustomParticleData(customData2, ParticleSystemCustomData.Custom2);
        }

        ///
        ParticleSystem.SetParticles(particles, ParticleCount);

        ///
        OnFinishedManipulatingParticles();
    }

    protected Vector4 GetCustomData(ParticleSystemCustomData dataStream)
    {
        switch (dataStream)
        {
            case ParticleSystemCustomData.Custom1:
                return customData1[CurrentParticleId];
            case ParticleSystemCustomData.Custom2:
                return customData2[CurrentParticleId];
            default:
                throw new System.NotImplementedException();
        }
    }

    protected void SetCustomData(Vector4 value, ParticleSystemCustomData dataStream)
    {
        switch (dataStream)
        {
            case ParticleSystemCustomData.Custom1:
                customData1[CurrentParticleId] = value;
                customDataDirtyFlag1 = true;
                break;
            case ParticleSystemCustomData.Custom2:
                customData2[CurrentParticleId] = value;
                customDataDirtyFlag2 = true;
                break;
            default:
                throw new System.NotImplementedException();
        }
    }

    protected Vector3 GetWorldPosition(ParticleSystem.Particle particle)
    {
        ///
        Vector3 worldPos;

        ///
        var simSpace = ParticleSystem.main.simulationSpace;

        ///
        if (simSpace == ParticleSystemSimulationSpace.World)
        {
            worldPos = particle.position;
        }
        else if (simSpace == ParticleSystemSimulationSpace.Local)
        {
            worldPos = particle.position + transform.position;
        }
        else if (simSpace == ParticleSystemSimulationSpace.Custom)
        {
            throw new System.NotImplementedException();
        }
        else
        {
            throw new System.NotImplementedException();
        }

        ///
        return worldPos;
    }
}
