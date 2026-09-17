using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GD
{
    public class TransformProgressBar : ProgressBar
    {
        [SerializeField]
        private Vector2 fullPosition;
        [SerializeField]
        private Vector2 fullScale;
        [SerializeField]
        private Vector2 emptyPosition;
        [SerializeField]
        private Vector2 emptyScale;

        [Header("Debug")]
        [SerializeField, Range(0f, 1f)]
        private float testValue;

        private float value;

        public override float Value
        {
            get => value;
            protected set
            {
                this.value = value;
            }
        }

        protected override void DisplayValue(float value)
        {
            Vector3 pos = Vector2.Lerp(emptyPosition, fullPosition, value);
            pos.z = transform.localPosition.z;
            var scale = Vector2.Lerp(emptyScale, fullScale, value);

            ///
            transform.localPosition = pos;
            transform.localScale = scale;
        }

        protected void OnEnable()
        {
            DisplayValue(value);
        }

#if UNITY_EDITOR
        protected void Reset()
        {
            fullPosition = transform.localPosition;
            fullScale = transform.localScale;
        }

        [ContextMenu("Editor_SetCurrentAsFull")]
        private void Editor_SetCurrentAsFull()
        {
            ///
            UnityEditor.Undo.RecordObject(this, "Editor_SetCurrentAsFull");

            ///
            fullPosition = transform.localPosition;
            fullScale = transform.localScale;

            ///
            UnityEditor.EditorUtility.SetDirty(this);
        }

        [ContextMenu("Editor_SetCurrentAsEmpty")]
        private void Editor_SetCurrentAsEmpty()
        {
            ///
            UnityEditor.Undo.RecordObject(this, "Editor_SetCurrentAsEmpty");

            ///
            emptyPosition = transform.localPosition;
            emptyScale = transform.localScale;

            ///
            UnityEditor.EditorUtility.SetDirty(this);
        }

        [ContextMenu("Editor_ViewTestValue")]
        private void Editor_ViewTestValue()
        {
            DisplayValue(testValue);
        }
#endif
    }

}