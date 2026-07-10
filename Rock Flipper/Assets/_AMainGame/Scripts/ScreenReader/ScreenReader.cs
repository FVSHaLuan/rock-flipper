using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BT.Systems.ScreenReaders
{
    [RequireComponent(typeof(Camera))]
    public class ScreenReader : MonoBehaviourWithInit
    {
        public event System.Action OnBeforeRead;

        private const string MatrixProperty = "_VertexTransform";
        private const string OutTextureColorProperty = "_OutTexColor";
        private const string ClearColorProperty = "_ClearColor";
        private const string AlphaTextureName = "_AlphaTexture";
        private const string SoftEdgeName = "_SoftEdge";

        [SerializeField]
        private Shader shader;

        [Space]
        [SerializeField]
        private RenderTexture renderTexture;

        [Space]
        [SerializeField]
        private bool clearBeforeRead;
        [SerializeField]
        private Color clearColor;
        [SerializeField]
        private Texture2D alphaTexture;
        [SerializeField, Range(0, 1)]
        private float softEdge = 0.95f;

        [Space]
        [SerializeField]
        private Color outTextureColor;

        private Matrix4x4 matrix = Matrix4x4.identity;

        private Material material;

        public Matrix4x4 Matrix
        {
            get => matrix;

            set
            {
                matrix = value;
                material.SetMatrix(MatrixProperty, matrix);
            }
        }

        public RenderTexture RenderTexture { get => renderTexture; set => renderTexture = value; }

        protected void OnRenderImage(RenderTexture src, RenderTexture dest)
        {
            ///
            if (RenderTexture == null)
            {
                return;
            }

            ///
            if (material == null)
            {
                InitMaterial();
            }

            ///
            OnBeforeRead?.Invoke();

            ///
            if (clearBeforeRead)
            {
                Clear();
            }

            ///
            material.SetColor(OutTextureColorProperty, outTextureColor);
            material.SetFloat(SoftEdgeName, softEdge);

            ///
            Graphics.Blit(src, RenderTexture, material, 1);

            ///
            Graphics.Blit(src, dest);
        }

        private void InitMaterial()
        {
            ///
            material = new Material(shader);

            ///
            material.SetTexture(AlphaTextureName, alphaTexture);
        }

        private void Clear()
        {
            ///
            material.SetColor(ClearColorProperty, clearColor);

            ///
            Graphics.Blit(null, RenderTexture, material, 0);
        }

#if UNITY_EDITOR
        protected void Reset()
        {
            var cam = GetComponent<Camera>();
            cam.cullingMask = 0;
            cam.clearFlags = CameraClearFlags.Nothing;
        }
#endif
    }

}