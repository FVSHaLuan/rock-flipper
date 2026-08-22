using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent, RequireComponent(typeof(UnifiedColoredObject))]
public class RandomAlphaView : OneTimeView
{
    [SerializeField]
    private float minAlpha = 0;
    [SerializeField]
    private float maxAlpha = 1;

    private UnifiedColoredObject unifiedColoredObject;

    protected override bool Init()
    {
        ///
        unifiedColoredObject = GetComponent<UnifiedColoredObject>();

        ///
        return base.Init();
    }

    public override void UpdateView()
    {
        ///
        TryInit();

        ///
        var alpha = Random.Range(minAlpha, maxAlpha);

        ///
        unifiedColoredObject.SetAlpha(alpha);
    }
}
