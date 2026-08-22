using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleColoredObject : UnifiedColoredObject
{
    protected override void SetColor(Color color)
    {
        var particleSystem = GetComponent<ParticleSystem>();
        var mainModule = particleSystem.main;
        mainModule.startColor = color;
    }
}
