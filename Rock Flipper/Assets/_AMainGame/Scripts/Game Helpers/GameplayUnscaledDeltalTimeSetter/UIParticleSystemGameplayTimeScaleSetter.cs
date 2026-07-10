using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BSBUIParticleSystem))]
[DisallowMultipleComponent]
public class UIParticleSystemGameplayTimeScaleSetter : GameplayTimeScaleSetter
{
    private BSBUIParticleSystem uiParticleSystem;

    protected override void ExtendedAwake()
    {
        ///
        base.ExtendedAwake();

        ///
        uiParticleSystem = GetComponent<BSBUIParticleSystem>();
    }

    protected override void Set(bool useUnscaledTime)
    {
        uiParticleSystem.UseUnscaledDeltaTime = useUnscaledTime;
    }
}
