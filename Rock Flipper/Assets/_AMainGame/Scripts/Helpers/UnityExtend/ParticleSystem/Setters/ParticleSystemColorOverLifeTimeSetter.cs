using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemColorOverLifeTimeSetter : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem targetParticleSystem;
    [SerializeField]
    private ParticleSystem.MinMaxGradient colorOverLifeTime;

    [ContextMenu("Set")]
    public void Set()
    {
        ///
        var module = targetParticleSystem.colorOverLifetime;

        ///
        module.color = colorOverLifeTime;
    }
}
