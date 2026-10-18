using Agame.FeatureBranching;
using Agame.Localization;
using FH.Core.Gameplay.HelperComponent;
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
        [SerializeField, Tooltip("Optional. Blinks the Preferred indicator's text while the Preferred background isn't the active background.")]
        private Blinker preferredTextBlinker;
        [SerializeField, Tooltip("Optional. Shown by the unlock animation of UnlockBackgroundButton while it stops at this item.")]
        private GameObject flashBackground;
        [SerializeField, Tooltip("Optional. Shows how many Unique items this background has dropped: \"0/1\" or \"1/1\", or \"0/1 (N/A in demo)\" in demo and playtest builds once another background has dropped one.")]
        private UnifiedText uniqueItemCountText;

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
            subscribedRunData.OnBackgroundDroppedUniqueChest += RunData_OnBackgroundStateChanged;

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
                subscribedRunData.OnBackgroundDroppedUniqueChest -= RunData_OnBackgroundStateChanged;
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

        public void SetFlashing(bool flashing)
        {
            if (flashBackground != null)
            {
                flashBackground.SetActive(flashing);
            }
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
                selectButton.interactable = true;
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
            if (preferredTextBlinker != null)
            {
                preferredTextBlinker.IsBlinking = preferred && !active;
            }
            if (uniqueItemCountText != null)
            {
                uniqueItemCountText.SetText(GetUniqueItemCountText(backgroundId));
            }
        }

        private string GetUniqueItemCountText(int backgroundId)
        {
            if (RunData.HasBackgroundDroppedUniqueChest(backgroundId))
            {
                return "1/1";
            }

            // Demo and playtest builds drop only 1 Unique chest per save (UniqueChestDropRules).
            if (VersionBranchInfo.IsPlaytestOrDemo && RunData.UniqueChestDropCount > 0)
            {
                return $"0/1 ({LocalizedStrings.Instance.naInDemo.GetLocalizedString()})";
            }

            ///
            return "0/1";
        }
    }

}
