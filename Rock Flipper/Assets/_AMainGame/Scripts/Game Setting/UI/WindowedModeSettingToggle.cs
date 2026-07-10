using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle)), DisallowMultipleComponent]
public class WindowedModeSettingToggle : MonoBehaviour
{
    private Toggle toggle;

    protected void Awake()
    {
        toggle = GetComponent<Toggle>();
    }

    public void OnEnable()
    {
        ///
        toggle.isOn =FullscreenToggle.IsWindowedMode();

        ///
        toggle.onValueChanged.AddListener(OnToggleChangedValue);
    }

    public void OnDisable()
    {
        ///
        toggle.onValueChanged.RemoveListener(OnToggleChangedValue);
    }

    private void OnToggleChangedValue(bool value)
    {
        SetWindowedMode(toggle.isOn);
    }    

    private void SetWindowedMode(bool value)
    {
        if (value)
        {
            Screen.fullScreenMode = FullScreenMode.Windowed;
        }
        else
        {
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        }
    }
}
