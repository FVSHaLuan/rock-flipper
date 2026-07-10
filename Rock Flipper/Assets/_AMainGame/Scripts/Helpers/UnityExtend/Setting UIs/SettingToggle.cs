using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public abstract class SettingToggle : MonoBehaviourWithInit
{
    private Toggle toggle;

    protected abstract bool GetValue();
    protected abstract void SetValue(bool value);

    protected override bool Init()
    {
        ///
        toggle = GetComponent<Toggle>();

        ///
        return base.Init();
    }

    protected void OnDisable()
    {
        ///
        toggle.onValueChanged.RemoveListener(OnValueChanged);
    }

    protected virtual void OnEnable()
    {
        toggle.isOn = GetValue();

        ///
        toggle.onValueChanged.AddListener(OnValueChanged);
    }

    private void OnValueChanged(bool newValue)
    {
        SetValue(newValue);
    }

    protected void SetToggleValueSilently(bool value)
    {
        toggle.SetIsOnWithoutNotify(value);
    }
}
