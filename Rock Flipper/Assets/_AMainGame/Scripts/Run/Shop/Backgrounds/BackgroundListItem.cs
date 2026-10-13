using UnityEngine;
using UnityEngine.UI;

namespace Agame.Run.Shop
{
    /// <summary>
    /// One entry of the Background list. Shows the background whose id equals this item's sibling index
    /// under the list, and sets it as the active background when its select button is clicked.
    /// </summary>
    public class BackgroundListItem : ExtendedMonoBehaviourRun
    {
        [SerializeField]
        private GameObject lockedView;
        [SerializeField]
        private GameObject unlockedView;
        [SerializeField]
        private Button selectButton;
        [SerializeField]
        private UnifiedText nameText;

        [Space]
        [SerializeField, Tooltip("Optional. Shown while this background is the active background.")]
        private GameObject activeIndicator;
        [SerializeField, Tooltip("Optional. Shown while this background is the Preferred background.")]
        private GameObject preferredIndicator;

        private RunData subscribedRunData;
        private bool started;

        public int BackgroundId => transform.GetSiblingIndex();

        protected void Start()
        {
            ///
            selectButton.onClick.AddListener(SelectButton_OnClick);

            ///
            subscribedRunData = RunData;
            subscribedRunData.OnActiveBackgroundIdChanged += RunData_OnBackgroundStateChanged;
            subscribedRunData.OnPreferredBackgroundIdChanged += RunData_OnBackgroundStateChanged;
            subscribedRunData.OnBackgroundUnlocked += RunData_OnBackgroundStateChanged;

            ///
            started = true;
            ViewBackgroundState();
        }

        protected void OnEnable()
        {
            if (started)
            {
                ViewBackgroundState();
            }
        }

        protected void OnDestroy()
        {
            if (subscribedRunData != null)
            {
                subscribedRunData.OnActiveBackgroundIdChanged -= RunData_OnBackgroundStateChanged;
                subscribedRunData.OnPreferredBackgroundIdChanged -= RunData_OnBackgroundStateChanged;
                subscribedRunData.OnBackgroundUnlocked -= RunData_OnBackgroundStateChanged;
            }
        }

        private void RunData_OnBackgroundStateChanged(int backgroundId)
        {
            ViewBackgroundState();
        }

        private void SelectButton_OnClick()
        {
            RunData.ActiveBackgroundId = BackgroundId;
        }

        public void ViewBackgroundState()
        {
            var backgroundId = BackgroundId;
            var backgroundManager = RunEntry.combatBackgroundManager;
            var unlocked = RunData.IsBackgroundUnlocked(backgroundId);
            var active = unlocked && RunData.ActiveBackgroundId == backgroundId;

            // With only 1 background unlocked there is no Preferred background.
            var preferred = unlocked
                && RunData.UnlockedBackgroundCount >= 2
                && RunData.PreferredBackgroundId == backgroundId;

            ///
            lockedView.SetActive(!unlocked);
            unlockedView.SetActive(unlocked);
            if (unlocked)
            {
                nameText.SetText(backgroundManager.HasBackground(backgroundId)
                    ? backgroundManager.GetBackground(backgroundId).BackgroundName
                    : $"Missing background {backgroundId}");
                selectButton.interactable = !active;
            }

            ///
            if (activeIndicator != null)
            {
                activeIndicator.SetActive(active);
            }
            if (preferredIndicator != null)
            {
                preferredIndicator.SetActive(preferred);
            }
        }
    }

}
