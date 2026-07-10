using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Agame.PointerInput
{
    public class PointerEnterAndExitEvents : MonoBehaviourWithInit, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField]
        private UnityEvent onPointerEnter;
        [SerializeField]
        private UnityEvent onPointerExit;

        private Selectable selectable;

        protected override bool Init()
        {
            ///
            selectable = GetComponent<Selectable>();

            ///
            return base.Init();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            ///
            if (selectable != null
                && !selectable.IsInteractable())
            {
                return;
            }

            ///
            onPointerEnter?.Invoke();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            ///
            if (selectable != null
                && !selectable.IsInteractable())
            {
                return;
            }

            ///
            onPointerExit?.Invoke();
        }
    }

}