using I2.Loc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ValueDisplayerUnifiedLocalized<T> : ValueDisplayerUnified<T> where T : System.IComparable<T>
{
    [SerializeField]
    private LocalizedString displayFormat;

    protected override void OnEnable()
    {
        ///
        base.OnEnable();

        ///
        LocalizationManager.OnLocalizeEvent += LocalizationManager_OnLocalizeEvent;
    }

    protected virtual void OnDisable()
    {
        LocalizationManager.OnLocalizeEvent -= LocalizationManager_OnLocalizeEvent;
    }

    private void LocalizationManager_OnLocalizeEvent()
    {
        Refresh();
    }

    protected override string GetString(T value)
    {
        return string.Format(displayFormat, value);
    }
}
