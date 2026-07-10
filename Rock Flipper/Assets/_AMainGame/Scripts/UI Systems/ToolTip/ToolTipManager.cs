using UnityEngine;
using UnityEngine.InputSystem;

namespace Agame.UI.ToolTips
{
    public class ToolTipManager : ExtendedMonoBehaviour
    {
        public const int InvalidRequestId = int.MinValue;

        [SerializeField]
        private ToolTip defaultToolTip;
        [SerializeField]
        private RectTransform root;

        private int nextRequestId = int.MinValue + 1;
        private int currentRequestId = InvalidRequestId;
        private ToolTipRequest currentRequest;
        private ToolTip currentToolTip;

        public int CurrentRequestId { get => currentRequestId; }

        public int Show(ToolTipRequest request)
        {
            ///
            Hide(currentRequestId);

            ///
            currentRequest = request;
            currentRequestId = ++nextRequestId;

            ///
            ShowCurrentRequest();

            ///
            return currentRequestId;
        }

        public bool Hide(int requestId)
        {
            if (requestId != currentRequestId
                && requestId == InvalidRequestId)
            {
                return false;
            }

            ///
            currentToolTip?.TryReturnToPoolAndDeactivate();

            ///
            currentRequestId = InvalidRequestId;
            currentToolTip = null;

            ///
            return true;
        }

        public void UpdateTooltip(int requestId, ToolTipRequest newRequest)
        {
            if (requestId != currentRequestId)
            {
                return;
            }

            ///
            currentRequest = newRequest;

            ///
            UpdateCurrentTooltip();
        }

        public bool HideTemporarily(int requestId)
        {
            if (requestId != currentRequestId
                && requestId == InvalidRequestId)
            {
                return false;
            }

            ///
            currentToolTip?.gameObject.SetActive(false);

            ///
            return true;
        }

        public void Show(int requestId)
        {
            if (requestId != currentRequestId)
            {
                return;
            }

            ///
            currentToolTip?.gameObject.SetActive(true);
        }

        private void ShowCurrentRequest()
        {
            var prototype = currentRequest.toolTipPrototype ?? defaultToolTip;
            currentToolTip = generalPool.TakeInstance(prototype, this);

            ///
            UpdateCurrentTooltip();
        }

        private void UpdateCurrentTooltip()
        {
            currentToolTip.SetContent(currentRequest);

            ///
            currentToolTip.gameObject.SetActive(true);
            currentToolTip.transform.SetParent(root, false);
            currentToolTip.transform.localScale = Vector3.one;

            ///
            UpdateCurrentToolTipPosition();
        }

        protected void Update()
        {
            if (currentRequestId == InvalidRequestId)
            {
                return;
            }

            ///
            if (currentRequest.updatePositionEveryFrame)
            {
                UpdateCurrentToolTipPosition();
            }

            ///
            if (Mouse.current != null
                && !currentRequest.doNotHideOnMouseDown)
            {
                if (Mouse.current.leftButton.wasPressedThisFrame)
                {
                    HideTemporarily(currentRequestId);
                }
                if (Mouse.current.leftButton.wasReleasedThisFrame)
                {
                    Show(currentRequestId);
                }
            }
        }

        private Vector2 GetCenterPosition()
        {
            if (currentRequest.useMouseCursor && Mouse.current != null)
            {
                var mousePosition = entry.GetPointerPositionViaConversionCamera();
                return mousePosition;
            }
            else if (currentRequest.centerTransform != null)
            {
                return currentRequest.centerTransform.position;
            }
            else
            {
                return Vector2.zero;
            }
        }

        private Vector2 GetToolTipBottomLeftAnchorPosition(Vector2 centerPoint)
        {
            if (currentRequest.toolTipBottomLeftAnchor == null
                || currentRequest.useMouseCursor)
            {
                return centerPoint + currentRequest.mouseCursorOffset * new Vector2(-1, -1);
            }
            else
            {
                return currentRequest.toolTipBottomLeftAnchor.position;
            }
        }

        private Vector2 GetToolTipTopLeftAnchorPosition(Vector2 centerPoint)
        {
            if (currentRequest.toolTipTopLeftAnchor == null
                || currentRequest.useMouseCursor)
            {
                return centerPoint + currentRequest.mouseCursorOffset * new Vector2(-1, 1);
            }
            else
            {
                return currentRequest.toolTipTopLeftAnchor.position;
            }
        }

        private Vector2 GetToolTipBottomRightAnchorPosition(Vector2 centerPoint)
        {
            if (currentRequest.toolTipBottomRightAnchor == null
                || currentRequest.useMouseCursor)
            {
                return centerPoint + currentRequest.mouseCursorOffset * new Vector2(1, -1);
            }
            else
            {
                return currentRequest.toolTipBottomRightAnchor.position;
            }
        }

        private Vector2 GetToolTipTopRightAnchorPosition(Vector2 centerPoint)
        {
            if (currentRequest.toolTipTopRightAnchor == null
                || currentRequest.useMouseCursor)
            {
                return centerPoint + currentRequest.mouseCursorOffset * new Vector2(1, 1);
            }
            else
            {
                return currentRequest.toolTipTopRightAnchor.position;
            }
        }

        private void UpdateCurrentToolTipPosition()
        {
            var centerPosition = GetCenterPosition();

            ///
            if (centerPosition.x >= 0)
            {
                if (centerPosition.y >= 0)
                {
                    currentToolTip.RectTransform.pivot = new Vector2(1, 1);
                    currentToolTip.transform.position = GetToolTipBottomLeftAnchorPosition(centerPosition);
                    // backgroundImage.transform.localScale = new Vector3(1, 1, 1);
                }
                else
                {
                    currentToolTip.RectTransform.pivot = new Vector2(1, 0);
                    currentToolTip.transform.position = GetToolTipTopLeftAnchorPosition(centerPosition);
                    // backgroundImage.transform.localScale = new Vector3(-1, 1, 1);
                }
            }
            else
            {
                if (centerPosition.y >= 0)
                {
                    currentToolTip.RectTransform.pivot = new Vector2(0, 1);
                    currentToolTip.transform.position = GetToolTipBottomRightAnchorPosition(centerPosition);
                    // backgroundImage.transform.localScale = new Vector3(-1, 1, 1);
                }
                else
                {
                    currentToolTip.RectTransform.pivot = new Vector2(0, 0);
                    currentToolTip.transform.position = GetToolTipTopRightAnchorPosition(centerPosition);
                    // backgroundImage.transform.localScale = new Vector3(1, 1, 1);
                }
            }
        }
    }

}