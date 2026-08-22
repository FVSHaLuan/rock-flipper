using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent, RequireComponent(typeof(UnifiedText))]
public abstract class OneTimeTextDisplayer<T> : OneTimeView
{
    [SerializeField, TextArea]
    private string format = "{0}";

    private UnifiedText unifiedText;

    protected override bool Init()
    {
        ///
        unifiedText = GetComponent<UnifiedText>();

        ///
        return base.Init();
    }

    protected abstract T GetValue();

    public override void UpdateView()
    {
        ///
        TryInit();

        ///
        unifiedText.Text = string.Format(format, GetValue());
    }
}
