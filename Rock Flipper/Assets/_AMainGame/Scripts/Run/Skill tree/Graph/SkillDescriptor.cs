using Agame.Run.Stats.Agents;
using UnityEngine;

namespace Agame.Run
{
    [RequireComponent(typeof(BuildAgent))]
    public class SkillDescriptor : MonoBehaviour
    {
        [SerializeField, TextArea]
        private string descriptionFormat;
        [SerializeField, TextArea]
        private string extraDescription;

        [Space]
        [SerializeField]
        private float buildValueMultiplier;

        public string GetDescription(double buildValue)
        {
            return string.Format(descriptionFormat, buildValue * buildValueMultiplier);
        }

        public string GetExtraDescription()
        {
            return extraDescription;
        }

    }

}