using UnityEngine;
using System.Collections;

namespace FH.Core.HelperComponent
{
    [RequireComponent(typeof(Renderer))]
    public class TextureScroller : ExtendedMonoBehaviour
    {
        [SerializeField, HideInInspector]
        private Renderer targetRenderer;

        [SerializeField]
        private Vector2 speed = Vector2.left;

        [Space]
        [SerializeField]
        private bool useGameplayUnscaledTime;

        private Material mainMaterial;

        public Vector2 Speed { get => speed; set => speed = value; }

        protected void OnEnable()
        {
            mainMaterial = targetRenderer.material;
        }

        public void Update()
        {
            ///
            var effectiveTimeScale = useGameplayUnscaledTime ? GameplayUnscaledDeltaTime : Time.deltaTime;

            ///
            Vector2 newOffset = mainMaterial.mainTextureOffset + speed * effectiveTimeScale;
            newOffset.x = Mathf.Repeat(newOffset.x, 1);
            newOffset.y = Mathf.Repeat(newOffset.y, 1);
            mainMaterial.mainTextureOffset = newOffset;
        }

        public void Reset()
        {
            targetRenderer = GetComponent<Renderer>();
        }
    }

}