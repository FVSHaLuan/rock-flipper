using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BT
{
    [RequireComponent(typeof(MeshRenderer))]
    public class CircularBarRenderer : MonoBehaviourWithInit
    {
        private Material material;

        protected override bool Init()
        {
            ///
            var quad = GetComponent<MeshRenderer>();

            ///
            material = quad.material;

            ///
            return base.Init();
        }

        public void SetFillColor(Color color)
        {
            ///
            TryInit();

            ///
            material.SetColor("_FillColor", color);
        }

        public void SetBackgroundColor(Color color)
        {
            ///
            TryInit();

            ///
            material.SetColor("_BackColor", color);
        }

        public void SetOutlineColor(Color color)
        {
            ///
            TryInit();

            ///
            material.SetColor("_OutlineColor", color);
        }

        public void SetSizes(float radius, float ringWidth, float outlineWidth)
        {
            TryInit();

            ///
            var quadScale = radius / 0.5f;
            var effRingWidth = ringWidth / quadScale;
            var effOutlineWidth = outlineWidth / quadScale;

            ///
            material.SetFloat("_OutlineWidth", effOutlineWidth);
            material.SetFloat("_OuterRadius", 1 - effOutlineWidth);
            material.SetFloat("_InnerRadius", 1f - effOutlineWidth - effRingWidth * 2);

            ///
            transform.localScale = Vector3.one * quadScale;
        }

        public void SetProgress(float progress)
        {
            ///
            TryInit();

            ///
            material.SetFloat("_Frac", progress);
        }
    }
}