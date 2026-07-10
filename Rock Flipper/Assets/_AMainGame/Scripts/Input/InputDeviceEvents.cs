using UnityEngine;
using UnityEngine.Events;

public class InputDeviceEvents : ExtendedMonoBehaviour
{
    [SerializeField]
    private UnityEvent onGamePad;
    [SerializeField]
    private UnityEvent onMouseAndKeyboard;

    protected void OnDisable()
    {
        entry.inputManager.OnActiveInputDeviceChanged -= InputManager_OnActiveInputDeviceChanged;
    }

    protected void OnEnable()
    {
        InvokeEvent();

        ///
        entry.inputManager.OnActiveInputDeviceChanged += InputManager_OnActiveInputDeviceChanged;
    }

    private void InputManager_OnActiveInputDeviceChanged()
    {
        InvokeEvent();
    }

    private void InvokeEvent()
    {
        switch (entry.inputManager.ActiveSimplifiedInputDevice.deviceType)
        {
            case SimplifiedInputDeviceType.MouseAndKeyboard:
                onMouseAndKeyboard?.Invoke();
                break;
            case SimplifiedInputDeviceType.Gamepad:
                onGamePad?.Invoke();
                break;
            default:
                throw new System.NotImplementedException();
        }
    }
}
