using Agame.Run.Stats.Agents;
using UnityEngine;

namespace Agame.Run
{
    [RequireComponent(typeof(BuildAgent))]
    public class SkillMetaData : MonoBehaviour
    {
        [Space]
        [SerializeField]
        private Sprite icon;
        [SerializeField]
        private Sprite subIcon;
        [SerializeField, Tooltip("Skill's title will be displayed as [titleGroup] - [title]")]
        private string titleGroup;
        [SerializeField, Tooltip("Skill's title will be displayed as [titleGroup] - [title]")]
        private string title;

        public Sprite Icon { get => icon; }
        public Sprite SubIcon => subIcon;
        public string TitleGroup { get => titleGroup; }
        public string Title { get => title; }
    }

}