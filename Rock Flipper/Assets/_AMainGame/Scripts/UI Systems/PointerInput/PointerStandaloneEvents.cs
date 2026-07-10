using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Agame.PointerInput
{
    public class PointerStandaloneEvents : MonoBehaviour
    {
        [SerializeField]
        private UnityEvent onMouseLeftDown;
        [SerializeField]
        private UnityEvent onMouseLeftUp;

        [Space]
        [SerializeField]
        private UnityEvent onMouseRightDown;
        [SerializeField, FormerlySerializedAs("onMouseRighttUp")]
        private UnityEvent onMouseRightUp;

        protected void Update()
        {
            // Mouse left down
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                onMouseLeftDown?.Invoke();
            }

            // Mouse left up
            if (Mouse.current.leftButton.wasReleasedThisFrame)
            {
                onMouseLeftUp?.Invoke();
            }

            // Mouse right down
            if (Mouse.current.rightButton.wasPressedThisFrame)
            {
                onMouseRightDown?.Invoke();
            }

            // Mouse right up
            if (Mouse.current.rightButton.wasReleasedThisFrame)
            {
                onMouseRightUp?.Invoke();
            }
        }
    }

}