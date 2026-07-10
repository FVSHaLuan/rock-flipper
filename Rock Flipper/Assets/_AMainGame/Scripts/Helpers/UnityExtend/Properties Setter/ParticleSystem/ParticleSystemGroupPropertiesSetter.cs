using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class ParticleSystemGroupPropertiesSetter : MonoBehaviourWithInit
{
    [SerializeField]
    private bool includeInactiveChildren;

    [Space]
    [SerializeField]
    private List<ParticleSystem.MinMaxCurve> valuePresets = new List<ParticleSystem.MinMaxCurve>();

    private List<ParticleSystem> particleSystems = new List<ParticleSystem>();

    protected override bool Init()
    {
        ///
        UpdateList();

        ///
        return base.Init();
    }

    public void UpdateList()
    {
        GetComponentsInChildren(includeInactiveChildren, particleSystems);
    }

    public void SetGravityScale(float scale)
    {
        ///
        TryInit();

        ///
        foreach (var item in particleSystems)
        {
            var m = item.main;
            m.gravityModifier = scale;
        }
    }

    public void SetGravityScalePreset(int presetIndex)
    {
        ///
        TryInit();

        ///
        var valuePreset = valuePresets[presetIndex];

        ///
        foreach (var item in particleSystems)
        {
            var m = item.main;
            m.gravityModifier = valuePreset;
        }
    }
}
