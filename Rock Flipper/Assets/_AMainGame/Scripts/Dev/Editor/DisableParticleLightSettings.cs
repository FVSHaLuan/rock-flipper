using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class DisableParticleLightSettings
{
    [MenuItem("CONTEXT/ParticleSystem/DisableLights")]
    private static void DisableLights(MenuCommand menuCommand)
    {        
        // Prevent executing multiple times when right-clicking.
        if (Selection.objects.Length > 1)
        {
            if (menuCommand.context != (Selection.objects[0] as GameObject)?.GetComponent<ParticleSystem>())
            {
                return;
            }
        }

        ///
        foreach (var item in Selection.objects)
        {
            ///
            var gameObject = item as GameObject;

            ///
            if (gameObject == null)
            {
                continue;
            }

            ///
            var ps = gameObject.GetComponent<ParticleSystem>();

            ///
            if (ps == null)
            {
                continue;
            }

            ///
            DisableLights(ps);
        }
    }

    private static void DisableLights(ParticleSystem particleSystem)
    {
        var renderer = particleSystem.GetComponent<ParticleSystemRenderer>();

        ///
        Undo.RecordObject(renderer, "Disable Light");

        ///
        renderer.lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.Off;
        renderer.reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.Off;
        renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        renderer.receiveShadows = false;

        ///
        EditorUtility.SetDirty(renderer);
    }
}
