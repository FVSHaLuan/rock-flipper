using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

namespace BT.PointerInput
{
    public class PointerEvents : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IPointerUpHandler, IPointerDownHandler
    {
        [Header("Enter and exit")]
        [SerializeField]
        private UnityEvent onPointerEnter;
        [SerializeField]
        private UnityEvent onPointerExit;

        [Space]
        [Header("Click")]
        [SerializeField]
        private UnityEvent onPointerClick;
        [SerializeField]
        private UnityEvent onPointerLeftClick;
        [SerializeField]
        private UnityEvent onPointerRightClick;

        [Space]
        [Header("Down")]
        [SerializeField]
        private UnityEvent onPointerDown;
        [SerializeField]
        private UnityEvent onPointerLeftDown;
        [SerializeField]
        private UnityEvent onPointerRightDown;

        [Space]
        [Header("Up")]
        [SerializeField]
        private UnityEvent onPointerUp;
        [SerializeField]
        private UnityEvent onPointerLeftUp;
        [SerializeField]
        private UnityEvent onPointerRightUp;

        private bool isPointerInside = false;

        public void OnPointerClick(PointerEventData eventData)
        {
            ///
            onPointerClick?.Invoke();

            ///
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                onPointerLeftClick?.Invoke();
            }
            else if (eventData.button == PointerEventData.InputButton.Right)
            {
                onPointerRightClick?.Invoke();
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            ///
            isPointerInside = true;

            ///
            onPointerEnter?.Invoke();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            ///
            isPointerInside = false;

            ///
            onPointerExit?.Invoke();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            ///
            onPointerUp?.Invoke();

            ///
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                onPointerLeftUp?.Invoke();
            }
            else if (eventData.button == PointerEventData.InputButton.Right)
            {
                onPointerRightUp?.Invoke();
            }
        }

        protected void OnDisable()
        {
            OnPointerExit(null);
        }

        protected void OnDestroy()
        {
            OnPointerExit(null);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            ///
            onPointerDown?.Invoke();

            ///
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                onPointerLeftDown?.Invoke();
            }
            else if (eventData.button == PointerEventData.InputButton.Right)
            {
                onPointerRightDown?.Invoke();
            }
        }

#if UNITY_EDITOR
        protected void OnValidate()
        {
            // Assert.IsNotNull(GetComponent<Renderer>());
        }
#endif
    }

}