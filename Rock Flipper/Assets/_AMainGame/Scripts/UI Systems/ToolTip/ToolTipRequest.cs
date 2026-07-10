using BT.UI.ToolTips;
using UnityEngine;

namespace BT.UI.ToolTips
{
    [System.Serializable]
    public struct ToolTipRequest
    {
        public ToolTip toolTipPrototype;
        public string mainText;
        public bool updatePositionEveryFrame;
        public bool useMouseCursor;
        public Vector2 mouseCursorOffset;
        public Transform centerTransform;
        public Transform toolTipTopRightAnchor;
        public Transform toolTipBottomRightAnchor;
        public Transform toolTipTopLeftAnchor;
        public Transform toolTipBottomLeftAnchor;
        public bool doNotHideOnMouseDown;
    }

}