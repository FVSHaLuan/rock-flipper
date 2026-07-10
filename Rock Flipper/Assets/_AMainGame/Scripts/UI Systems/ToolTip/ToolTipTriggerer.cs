using FH.Core.Gameplay.HelperComponent;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BT.UI.ToolTips
{
    public class ToolTipTriggerer : MonoBehaviourWithInit, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
    {
        public event System.Action OnBeforeShow;

        [SerializeField]
        private ToolTip toolTipPrototype;
        [SerializeField, TextArea]
        private string mainText;

        [Space]
        [SerializeField]
        private bool updatePositionEveryFrame;
        [SerializeField]
        private bool useMouseCursor;
        [SerializeField]
        private Vector2 mouseCursorOffset;

        [Space]
        [SerializeField]
        private bool doNotHideOnMouseDown;

        [Space]
        [SerializeField]
        private Transform centerTransform;
        [SerializeField]
        private Transform toolTipTopRightAnchor;
        [SerializeField]
        private Transform toolTipBottomRightAnchor;
        [SerializeField]
        private Transform toolTipTopLeftAnchor;
        [SerializeField]
        private Transform toolTipBottomLeftAnchor;

        private int requestId = ToolTipManager.InvalidRequestId;

        public bool IsShowing => requestId != ToolTipManager.InvalidRequestId
            && requestId == CommonEntry.CommonInstance.toolTipManager.CurrentRequestId;

        public string MainText
        {
            get => mainText;
            set
            {
                mainText = value;
                if (IsShowing)
                {
                    CommonEntry.CommonInstance.toolTipManager.UpdateTooltip(requestId, GetRequest());
                }
            }
        }

        protected void OnDisable()
        {
            Hide();
        }

        private ToolTipRequest GetRequest()
        {
            ToolTipRequest request = new ToolTipRequest
            {
                toolTipPrototype = toolTipPrototype,
                mainText = MainText,
                centerTransform = centerTransform,
                toolTipTopRightAnchor = toolTipTopRightAnchor,
                toolTipBottomRightAnchor = toolTipBottomRightAnchor,
                toolTipTopLeftAnchor = toolTipTopLeftAnchor,
                toolTipBottomLeftAnchor = toolTipBottomLeftAnchor,
                mouseCursorOffset = mouseCursorOffset,
                updatePositionEveryFrame = updatePositionEveryFrame,
                useMouseCursor = useMouseCursor,
                doNotHideOnMouseDown = doNotHideOnMouseDown
            };

            ///
            return request;
        }

        public void Show()
        {
            if (!IsShowing)
            {
                OnBeforeShow?.Invoke();
                requestId = CommonEntry.CommonInstance.toolTipManager.Show(GetRequest());
            }
        }

        public void Hide()
        {
            CommonEntry.CommonInstance.toolTipManager.Hide(requestId);
            requestId = ToolTipManager.InvalidRequestId;
        }

        public void OnDeselect(BaseEventData eventData)
        {
            Hide();
        }

        public void OnSelect(BaseEventData eventData)
        {
            Show();
        }

        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            Show();
        }

        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            Hide();
        }

#if UNITY_EDITOR
        [ContextMenu("Editor_CreateTransforms")]
        private void Editor_CreateTransforms()
        {
            ///
            UnityEditor.Undo.RegisterFullObjectHierarchyUndo(this.gameObject, "Editor_CreateTransforms");

            // Center
            if (centerTransform == null)
            {
                var centerGO = new GameObject("ToolTip_Center", typeof(RectTransform));
                centerGO.transform.SetParent(this.transform, false);
                centerTransform = centerGO.transform;
                centerGO.transform.localPosition = Vector3.zero;
                centerGO.transform.localScale = Vector3.one;
            }

            // Top Right
            if (toolTipTopRightAnchor == null)
            {
                var topRightGO = new GameObject("ToolTip_TopRightAnchor", typeof(RectTransform));
                topRightGO.transform.SetParent(this.transform, false);
                toolTipTopRightAnchor = topRightGO.transform;
                ((topRightGO.transform) as RectTransform).anchorMin = new Vector2(1f, 1f);
                ((topRightGO.transform) as RectTransform).anchorMax = new Vector2(1f, 1f);
                ((topRightGO.transform) as RectTransform).pivot = new Vector2(1f, 1f);
                ((topRightGO.transform) as RectTransform).anchoredPosition = Vector3.zero;
                topRightGO.transform.localScale = Vector3.one;
            }

            // Bottom Right
            if (toolTipBottomRightAnchor == null)
            {
                var bottomRightGO = new GameObject("ToolTip_BottomRightAnchor", typeof(RectTransform));
                bottomRightGO.transform.SetParent(this.transform, false);
                toolTipBottomRightAnchor = bottomRightGO.transform;
                ((bottomRightGO.transform) as RectTransform).anchorMin = new Vector2(1f, 0f);
                ((bottomRightGO.transform) as RectTransform).anchorMax = new Vector2(1f, 0f);
                ((bottomRightGO.transform) as RectTransform).pivot = new Vector2(1f, 0f);
                ((bottomRightGO.transform) as RectTransform).anchoredPosition = Vector3.zero;
                bottomRightGO.transform.localScale = Vector3.one;
            }

            // Top Left
            if (toolTipTopLeftAnchor == null)
            {
                var topLeftGO = new GameObject("ToolTip_TopLeftAnchor", typeof(RectTransform));
                topLeftGO.transform.SetParent(this.transform, false);
                toolTipTopLeftAnchor = topLeftGO.transform;
                ((topLeftGO.transform) as RectTransform).anchorMin = new Vector2(0f, 1f);
                ((topLeftGO.transform) as RectTransform).anchorMax = new Vector2(0f, 1f);
                ((topLeftGO.transform) as RectTransform).pivot = new Vector2(0f, 1f);
                ((topLeftGO.transform) as RectTransform).anchoredPosition = Vector3.zero;
                topLeftGO.transform.localScale = Vector3.one;
            }

            // Bottom Left
            if (toolTipBottomLeftAnchor == null)
            {
                var bottomRightGO = new GameObject("ToolTip_BottomLeftAnchor", typeof(RectTransform));
                bottomRightGO.transform.SetParent(this.transform, false);
                toolTipBottomLeftAnchor = bottomRightGO.transform;
                ((bottomRightGO.transform) as RectTransform).anchorMin = new Vector2(0f, 0f);
                ((bottomRightGO.transform) as RectTransform).anchorMax = new Vector2(0f, 0f);
                ((bottomRightGO.transform) as RectTransform).pivot = new Vector2(0f, 0f);
                ((bottomRightGO.transform) as RectTransform).anchoredPosition = Vector3.zero;
                bottomRightGO.transform.localScale = Vector3.one;
            }
        }
#endif
    }

}