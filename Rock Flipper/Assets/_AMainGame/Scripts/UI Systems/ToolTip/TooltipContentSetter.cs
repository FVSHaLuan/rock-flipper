using UnityEngine;

namespace Agame.UI.ToolTips
{
    [RequireComponent(typeof(ToolTipTriggerer))]
    [DisallowMultipleComponent]
    public abstract class TooltipContentSetter : ExtendedMonoBehaviour
    {
        protected ToolTipTriggerer ToolTipTriggerer { get; private set; }

        protected override void ExtendedAwake()
        {
            ///
            base.ExtendedAwake();

            ///
            ToolTipTriggerer = GetComponent<ToolTipTriggerer>();

            ///
            if (ToolTipTriggerer.IsShowing)
            {
                UpdateContent();
            }

            ///
            ToolTipTriggerer.OnBeforeShow += ToolTipTriggerer_OnBeforeShow;
        }

        private void ToolTipTriggerer_OnBeforeShow()
        {
            UpdateContent();
        }

        protected virtual void UpdateContent()
        {
            ///
            // Override in derived classes to set the tooltip content
        }
    }

}