using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
[DisallowMultipleComponent]
public class ParticleSystemGameplayTimeScaleSetter : GameplayTimeScaleSetter
{
    private new ParticleSystem particleSystem;

    protected override void ExtendedAwake()
    {
        ///
        particleSystem = GetComponent<ParticleSystem>();

        ///
        base.ExtendedAwake();
    }

    protected override void Set(bool useUnscaledTime)
    {
        var mainModule = particleSystem.main;
        mainModule.useUnscaledTime = useUnscaledTime;
    }
}
