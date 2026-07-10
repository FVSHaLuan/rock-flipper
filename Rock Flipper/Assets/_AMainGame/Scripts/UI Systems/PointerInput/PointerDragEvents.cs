using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

namespace Agame.PointerInput
{
    public class PointerDragEvents : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        [SerializeField]
        private bool disallowRightMouseDrag = false;

        [Space]
        [SerializeField]
        private UnityEvent onBeginDrag;
        [SerializeField]
        private UnityEvent onEndDrag;

        public void OnBeginDrag(PointerEventData eventData)
        {
            ///
            if (disallowRightMouseDrag && eventData.button == PointerEventData.InputButton.Right)
            {
                ///
                eventData.pointerDrag = null;

                ///
                return;
            }

            ///
            onBeginDrag?.Invoke();
        }

        public void OnDrag(PointerEventData eventData)
        {
            // throw new System.NotImplementedException();
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            onEndDrag?.Invoke();
        }
    }

}