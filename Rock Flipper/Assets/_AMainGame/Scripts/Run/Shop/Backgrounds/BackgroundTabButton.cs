using Agame.Run.Stats;
using UnityEngine;

namespace Agame.Run.Shop
{
    /// <summary>
    /// The tab button of the Background list. Hides what it leads to and locks its <see cref="TabButton"/>
    /// until the background feature is enabled.
    /// </summary>
    [RequireComponent(typeof(TabButton))]
    public class BackgroundTabButton : ExtendedMonoBehaviourRun
    {
        private const string HiddenText = "???";

        [SerializeField]
        private UnifiedText text;
        [SerializeField]
        private GameObject icon;

        private string enabledText;
        private TabButton tabButton;

        protected void OnDisable()
        {
            BuildStatsObject.OnEnabledBackgrounds -= BuildStatsObject_OnEnabledBackgrounds;
        }

        protected void OnEnable()
        {
            ///
            ViewEnabledState();

            ///
            BuildStatsObject.OnEnabledBackgrounds += BuildStatsObject_OnEnabledBackgrounds;
        }

        private void BuildStatsObject_OnEnabledBackgrounds()
        {
            ViewEnabledState();
        }

        private void ViewEnabledState()
        {
            ///
            if (tabButton == null)
            {
                tabButton = GetComponent<TabButton>();
            }
            if (enabledText == null)
            {
                enabledText = text.Text;
            }

            ///
            var enabled = BuildStats.BackgroundsEnabled;
            text.SetText(enabled ? enabledText : HiddenText);
            icon.SetActive(enabled);
            tabButton.IsLocked = !enabled;
        }
    }

}
