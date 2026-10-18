using Agame.Localization;
using Agame.Run.Stats.Agents;
using UnityEngine;
using UnityEngine.Localization;

namespace Agame.Run
{
    [DisallowMultipleComponent]
    public class SkillDescriptor : MonoBehaviour
    {
        [SerializeField, TextArea]
        private string descriptionFormat;
        [SerializeField, Tooltip("Overrides descriptionFormat when set. {0}: build value x buildValueMultiplier")]
        private LocalizedString localizedDescriptionFormat;
        [SerializeField, TextArea]
        private string extraDescription;
        [SerializeField, Tooltip("Overrides extraDescription when set")]
        private LocalizedString localizedExtraDescription;

        [Space]
        [SerializeField]
        private float buildValueMultiplier = 1;

        private string DescriptionFormat { get => localizedDescriptionFormat.IsValidAndNotEmpty() ? localizedDescriptionFormat.GetLocalizedString() : descriptionFormat; }

        public string GetDescription(double buildValue)
        {
            return string.Format(DescriptionFormat, buildValue * buildValueMultiplier);
        }

        public string GetExtraDescription()
        {
            return localizedExtraDescription.IsValidAndNotEmpty() ? localizedExtraDescription.GetLocalizedString() : extraDescription;
        }

    }

}
