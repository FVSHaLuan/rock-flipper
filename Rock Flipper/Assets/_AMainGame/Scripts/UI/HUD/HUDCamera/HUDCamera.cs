using FHC.Core.Architecture;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BT.UI
{
    [RequireComponent(typeof(Camera))]
    public class HUDCamera : ExtendedMonoBehaviour
    {
        [SerializeField, UnityLayer]
        private int dropItemLayer;

        private new Camera camera;

        private BalancerWithObjects viewDropItemBalancer = new BalancerWithObjects();

        protected override void ExtendedAwake()
        {
            ///
            base.ExtendedAwake();

            ///
            camera = GetComponent<Camera>();
        }

        public void AddHideDropItemLock(object @object)
        {
            ///
            viewDropItemBalancer.AddObject(@object);

            ///
            UpdateViewDropItem();
        }

        public void RemoveHideDropItemLock(object @object)
        {
            ///
            viewDropItemBalancer.RemoveObject(@object);

            ///
            UpdateViewDropItem();
        }

        private void UpdateViewDropItem()
        {
            ///
            var cullingMask = camera.cullingMask;

            ///
            if (viewDropItemBalancer.IsBalanced)
            {
                cullingMask = cullingMask | (1 << dropItemLayer);
            }
            else
            {
                cullingMask = cullingMask & ~(1 << dropItemLayer);
            }

            ///
            camera.cullingMask=cullingMask;
        }
    }

}