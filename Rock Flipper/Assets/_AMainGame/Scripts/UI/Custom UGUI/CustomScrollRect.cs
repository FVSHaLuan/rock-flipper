using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BT.UI
{
    public class CustomScrollRect : ScrollRect
    {
        [Header("CustomScrollRect")]
        [SerializeField]
        private float verticalScrollBarMinSize = -1;
        [SerializeField]
        private float horizontalScrollBarMinSize = -1;
        [SerializeField]
        private bool enableDragScroll = true;
        [SerializeField]
        private bool enableMouseScroll = true;

        public override void Rebuild(CanvasUpdate executing)
        {
            ///
            base.Rebuild(executing);

            ///
            UpdateScrollBarsMinSizes();
        }

        protected override void LateUpdate()
        {
            ///
            base.LateUpdate();

            ///
            UpdateScrollBarsMinSizes();
        }

        private void UpdateScrollBarsMinSizes()
        {
            ///
            if (verticalScrollbar && verticalScrollBarMinSize >= 0)
            {
                verticalScrollbar.size = Mathf.Max(verticalScrollbar.size, verticalScrollBarMinSize);
            }

            ///
            if (horizontalScrollbar && horizontalScrollBarMinSize >= 0)
            {
                horizontalScrollbar.size = Mathf.Max(horizontalScrollbar.size, horizontalScrollBarMinSize);
            }
        }

        public override void OnBeginDrag(PointerEventData eventData)
        {
            if (enableDragScroll)
            {
                base.OnBeginDrag(eventData);
            }
        }

        public override void OnDrag(PointerEventData eventData)
        {
            if (enableDragScroll)
            {
                base.OnBeginDrag(eventData);
            }
        }

        public override void OnEndDrag(PointerEventData eventData)
        {
            if (enableDragScroll)
            {
                base.OnBeginDrag(eventData);
            }
        }

        public override void OnScroll(PointerEventData data)
        {
            if (enableMouseScroll)
            {
                base.OnScroll(data);
            }
        }
    }

}