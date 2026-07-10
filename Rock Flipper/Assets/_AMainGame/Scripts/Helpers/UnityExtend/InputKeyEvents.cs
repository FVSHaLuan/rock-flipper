using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InputKeyEvents : MonoBehaviour
{
    [SerializeField]
    private Key key;

    [SerializeField]
    private UnityEvent onKeyDown;
    [SerializeField]
    private UnityEvent onKeyUp;

    protected void Update()
    {
        var inputKeyEvent = Keyboard.current?[key];
        if (inputKeyEvent != null)
        {
            if (inputKeyEvent.wasPressedThisFrame)
            {
                onKeyDown?.Invoke();
            }
            else if (inputKeyEvent.wasReleasedThisFrame)
            {
                onKeyUp?.Invoke();
            }
        }
    }
}
