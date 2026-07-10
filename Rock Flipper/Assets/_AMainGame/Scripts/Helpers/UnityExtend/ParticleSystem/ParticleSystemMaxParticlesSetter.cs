using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleSystemMaxParticlesSetter : MonoBehaviour
{
    [SerializeField]
    private int maxParticles = 1000;
    [SerializeField]
    private bool setInUpdate = false;

    private new ParticleSystem particleSystem;

    public int MaxParticles
    {
        get => maxParticles;
        set
        {
            maxParticles = value;
            Set();
        }
    }

    public void Set()
    {
        ///
        if (particleSystem == null)
        {
            particleSystem = GetComponent<ParticleSystem>();
        }

        ///
        var main = particleSystem.main;
        main.maxParticles = maxParticles;
    }

    protected void Update()
    {
        if (setInUpdate)
        {
            Set();
        }
    }

}
