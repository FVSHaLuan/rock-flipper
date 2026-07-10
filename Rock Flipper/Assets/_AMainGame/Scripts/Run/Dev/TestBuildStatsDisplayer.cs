using UnityEngine;

namespace BT.Run.Dev
{
    public class TestBuildStatsDisplayer : ExtendedMonoBehaviourRun
    {
        [SerializeField]
        private UnifiedText unifiedText;
        protected void OnEnable()
        {
            if (RunEntry.IsUsingTestBuildStats)
            {
                gameObject.SetActive(true);
                unifiedText.Text = $"Test BuildStats: {RunEntry.BaseBuildStatsName}";
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }

}