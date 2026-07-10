using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GD
{
    [RequireComponent(typeof(RectTransform))]
    public class UISizeProgressBar : ProgressBar
    {
        [SerializeField]
        private Vector2 startSize;
        [SerializeField]
        private Vector2 endSize;

        [Header("Test")]
        [SerializeField]
        private float editor_TestValue;

        private float value;

        private RectTransform rectTransform;

        public override float Value
        {
            get => value;
            protected set => this.value = value;
        }

        protected override void DisplayValue(float value)
        {
            ///
            var size = Vector2.Lerp(startSize, endSize, value);

            ///
            if (rectTransform == null)
            {
                rectTransform = GetComponent<RectTransform>();
            }

            ///
            rectTransform.sizeDelta = size;
        }

#if UNITY_EDITOR
        [ContextMenu("LogSize")]
        private void Editor_LogSize()
        {
            Debug.LogFormat("sizeDelta: {0}", GetComponent<RectTransform>().sizeDelta);
        }

        [ContextMenu("Editor_TestDisplayValue")]
        private void Editor_TestDisplayValue()
        {
            DisplayValue(editor_TestValue);
        }
#endif
    }
}
