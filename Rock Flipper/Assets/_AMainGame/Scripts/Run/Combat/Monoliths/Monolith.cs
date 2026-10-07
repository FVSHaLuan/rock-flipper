using UnityEngine;

namespace Agame.Run.Combat
{
    /// <summary>
    /// Shows the monolith only while it is unlocked, so its specific behaviour (e.g. <see cref="Prism"/>) doesn't need to check
    /// </summary>
    public class Monolith : ExtendedMonoBehaviourRun
    {
        [SerializeField]
        private MonolithType monolithType;

        protected void Start()
        {
            UpdateActiveState();

            ///
            RunEntry.skillTreeScreen.OnClosed += SkillTreeScreen_OnClosed;
        }

        protected void OnDestroy()
        {
            RunEntry.skillTreeScreen.OnClosed -= SkillTreeScreen_OnClosed;
        }

        private void SkillTreeScreen_OnClosed()
        {
            UpdateActiveState();
        }

        private void UpdateActiveState()
        {
            gameObject.SetActive(BuildStats.GetMonolithBuildStats(monolithType).unlocked);
        }
    }

}
