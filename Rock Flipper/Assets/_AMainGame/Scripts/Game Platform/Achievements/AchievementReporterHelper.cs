using UnityEngine;

namespace Agame.GamePlatform
{
    public class AchievementReporterHelper : ExtendedMonoBehaviour
    {
        [SerializeField]
        private AchievementConfig achievementConfig;

        [ContextMenu("Report"), PlayModeOnly]
        public void Report()
        {
            achievementConfig?.Report();
        }
    }

}