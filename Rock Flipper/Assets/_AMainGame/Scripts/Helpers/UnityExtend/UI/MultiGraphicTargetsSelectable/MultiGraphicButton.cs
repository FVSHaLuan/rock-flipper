using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GD
{
    public class MultiGraphicButton : Button
    {
        [Header("MultiGraphicToggle")]
        [SerializeField]
        private UnifiedText unifiedText;
        [SerializeField]
        List<Item> additionalTargetGraphics = new List<Item>();

        [System.Serializable]
        public struct Item
        {
            public Graphic graphic;
            public bool useCustomColors;
            public ColorBlock customColors;
        }

        public List<Item> AdditionalTargetGraphics
        {
            get
            {
                ///
                if (additionalTargetGraphics == null)
                {
                    additionalTargetGraphics = new List<Item>();
                }

                ///
                return additionalTargetGraphics;
            }
        }

        public UnifiedText Text => unifiedText;

        protected override void DoStateTransition(SelectionState state, bool instant)
        {
            ///
            if (transition != Transition.ColorTint)
            {
                base.DoStateTransition(state, instant);
            }
            else
            {
                ///
                var commonTargetColor = GetTargetColor(state, colors);
                StartColorTween(targetGraphic, commonTargetColor, instant);
            }

            ///
            for (int i = 0; i < AdditionalTargetGraphics.Count; i++)
            {
                ///
                var item = AdditionalTargetGraphics[i];

                ///
                if (item.graphic == null)
                {
                    continue;
                }

                ///
                var targetColor = GetTargetColor(state, item);
                StartColorTween(item.graphic, targetColor, instant);
            }
        }

        private Color GetTargetColor(SelectionState state, Item item)
        {
            ///
            if (!item.useCustomColors)
            {
                return GetTargetColor(state, colors);
            }

            ///
            return GetTargetColor(state, item.customColors);
        }

        private Color GetTargetColor(SelectionState state, ColorBlock colors)
        {
            ///
            Color targetColor;

            switch (state)
            {
                case SelectionState.Normal:
                    targetColor = colors.normalColor;
                    break;
                case SelectionState.Selected:
                    targetColor = colors.selectedColor;
                    break;
                case SelectionState.Highlighted:
                    targetColor = colors.highlightedColor;
                    break;
                case SelectionState.Pressed:
                    targetColor = colors.pressedColor;
                    break;
                case SelectionState.Disabled:
                    targetColor = colors.disabledColor;
                    break;
                default:
                    targetColor = Color.black;
                    break;
            }

            ///
            return targetColor;
        }

        private void StartColorTween(Graphic graphic, Color targetColor, bool instant)
        {
            if (graphic == null)
                return;

            ///
            graphic.CrossFadeColor(targetColor, instant ? 0f : colors.fadeDuration, true, true);
        }

        [ContextMenu("DoStateTransitionInstant")]
        public void DoStateTransitionInstant()
        {
            DoStateTransition(currentSelectionState, true);
        }

        public void InvokeOnclick()
        {
            onClick?.Invoke();
        }

        public void InvokeOnclickIfInteractable()
        {
            if (IsInteractable())
            {
                onClick?.Invoke();
            }
        }

#if UNITY_EDITOR
        protected override void Reset()
        {
            ///
            base.Reset();

            ///
            Editor_AutoFillAdditionalTargetGraphics(false);
        }

        private void Editor_AutoFillAdditionalTargetGraphics(bool registerUndoEntry)
        {
            ///
            if (registerUndoEntry)
            {
                UnityEditor.Undo.RecordObject(this, "AutoFillAdditionalTargetGraphics");
            }

            ///
            additionalTargetGraphics = new List<Item>();

            ///
            var graphics = GetComponentsInChildren<Graphic>(true);

            ///
            for (int i = 0; i < graphics.Length; i++)
            {
                ///
                var graphic = graphics[i];

                ///
                if (graphic == targetGraphic)
                {
                    continue;
                }

                ///
                var item = new Item()
                {
                    useCustomColors = false,
                    graphic = graphic,
                };

                ///
                additionalTargetGraphics.Add(item);
            }
        }

        [ContextMenu("Editor_AutoFillAdditionalTargetGraphics")]
        private void Editor_AutoFillAdditionalTargetGraphics()
        {
            Editor_AutoFillAdditionalTargetGraphics(true);
        }
#endif
    }

}