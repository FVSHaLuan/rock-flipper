using Agame.Localization;
using Agame.Run.Stats.Agents;
using UnityEngine;
using UnityEngine.Localization;

namespace Agame.Run
{
    [DisallowMultipleComponent]
    public class SkillMetaData : MonoBehaviour
    {
        [Space]
        [SerializeField, Tooltip("The icon representing the object the skill applies to")]
        private Sprite icon;
        [SerializeField, Tooltip("The sub-icon for the skill, hints at the skill's functionality or category")]
        private Sprite subIcon;
        [SerializeField, Tooltip("Skill's title will be displayed as [titleGroup] - [title]")]
        private string titleGroup;
        [SerializeField]
        private LocalizedString localizedTitleGroup;
        [SerializeField, Tooltip("Skill's title will be displayed as [titleGroup] - [title]")]
        private string title;
        [SerializeField]
        private LocalizedString localizedTitle;

        public Sprite Icon { get => icon; }
        public Sprite SubIcon => subIcon;
        public string TitleGroup { get => localizedTitleGroup.IsValidAndNotEmpty() ? localizedTitleGroup.GetLocalizedString() : titleGroup; }
        public string Title { get => localizedTitle.IsValidAndNotEmpty() ? localizedTitle.GetLocalizedString() : title; }
    }

}