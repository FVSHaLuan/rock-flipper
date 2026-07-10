using UnityEngine;

namespace BT.GamePlatform
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