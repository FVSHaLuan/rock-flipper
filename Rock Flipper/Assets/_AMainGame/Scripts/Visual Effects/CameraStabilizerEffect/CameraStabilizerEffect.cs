using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BT.Systems.CameraStabilizerEffect
{
    public class CameraStabilizerEffect : ExtendedMonoBehaviour
    {
        [SerializeField]
        private Shader shader;

        [Header("Blur")]
        [Range(0, 1)]
        public float blurryIntensity;
        [Range(-1, 1)]
        public float blurryMultiplier;

        [Header("View")]
        [Range(0, 1)]
        public float sizeScale = 1;
        [Range(-1, 1)]
        public float viewOffsetX;
        [Range(-1, 1)]
        public float viewOffsetY;

        [Space]
        [Range(0, 1)]
        public float originalOverride = 0;

        private Material material = null;

        protected override bool Init()
        {
            ///
            material = new Material(shader);

            ///
            return base.Init();
        }

        protected void OnRenderImage(RenderTexture src, RenderTexture dest)
        {
            // Set parameters
            //--Blur
            material.SetFloat("_Intensity", blurryIntensity);
            material.SetFloat("_Multiplier", blurryMultiplier);
            //--View
            material.SetFloat("_SizeScale", sizeScale);
            material.SetVector("_ViewOffset", new Vector2(viewOffsetX, viewOffsetY));
            //--
            material.SetFloat("_OriginalOverride", originalOverride);

            ///
            Graphics.Blit(src, dest, material);
        }
    }
}
