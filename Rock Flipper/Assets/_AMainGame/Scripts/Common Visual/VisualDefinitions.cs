using System.Collections.Generic;
using Agame.Dev;
using UnityEngine;
using UnityEngine.Serialization;

namespace Agame
{
    [CreateAssetMenu(menuName = "OV/SingleInstances/VisualDefinitions")]
    public class VisualDefinitions : ScriptableObjectWithInit
    {
        public Color normalTextColor;
        public Color notEnoughTextColor;

        [Space]
        public Color commonRarityForegroundColor;
        public Color uncommonRarityForegroundColor;
        public Color rareRarityForegroundColor;
        public Color epicRarityForegroundColor;
        public Color uniqueRarityForegroundColor;

        public static VisualDefinitions Instance
        {
            get
            {
#if UNITY_EDITOR
                if (!Application.isPlaying)
                {
                    return DevEntry.Instance.visualDefinitions;
                }
#endif

                return Entry.Instance.visualDefinitions;
            }
        }
    }

}