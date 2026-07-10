using FH.Core.Architecture.Pool;
using UnityEngine;

namespace BT.UI.ToolTips
{
    public class ToolTip : GeneralPoolMemberSimplified
    {
        [SerializeField]
        private UnifiedText mainText;

        public RectTransform RectTransform
        {
            get
            {
                return transform as RectTransform;
            }
        }

        public void SetContent(ToolTipRequest request)
        {
            mainText.SetText(request.mainText);
        }
    }

}