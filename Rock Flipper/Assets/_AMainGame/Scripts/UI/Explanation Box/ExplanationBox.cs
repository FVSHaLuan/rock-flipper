using FH.Core.Architecture.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace BT.UI
{
    [ExecuteInEditMode]
    public class ExplanationBox : GeneralPoolMemberSimplified
    {
        [SerializeField]
        private UnifiedText explanationText;

        [SerializeField]
        private Transform wrapper;

        [Space]
        [SerializeField]
        private SpriteRenderer glow;
        [SerializeField]
        private SpriteRenderer edge;
        [SerializeField]
        private SpriteRenderer background;
        [SerializeField]
        private RectTransform textRect;

        [Space]
        [SerializeField]
        private Vector2 glowSize = Vector2.one * 0.1f;
        [SerializeField]
        private Vector2 edgeSize = Vector2.one * 0.1f;
        [SerializeField]
        private Vector2 margin = Vector2.one * 0.1f;

        [Space]
        [SerializeField]
        private Vector2 grandSizeModifier = Vector2.zero;
        [SerializeField]
        private Vector2 pivot = Vector2.one / 2.0f;

        public Vector2 Pivot
        {
            get => pivot;

            set
            {
                ///
                pivot = value;
            }
        }

        public string Text
        {
            set
            {
                ///
                explanationText.Text = value;
            }
        }

        public Vector2 GrandSize { get; private set; }

        public void UpdateView()
        {
            //
            Vector2 size = new Vector2(textRect.sizeDelta.x, explanationText.PreferredHeight);

            ///
            size += margin;
            background.size = size;

            ///
            size += edgeSize;
            edge.size = size;

            ///
            size += glowSize;
            glow.size = size;

            ///
            GrandSize = size + grandSizeModifier;

            ///
            wrapper.transform.localPosition = new Vector2()
            {
                x = (0.5f - pivot.x) * GrandSize.x,
                y = (0.5f - pivot.y) * GrandSize.y,
            };
        }

#if UNITY_EDITOR
        protected void Update()
        {
            if (!Application.isPlaying)
            {
                UpdateView();
            }
        }
#endif
    }

}