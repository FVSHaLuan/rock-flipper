using UnityEngine;

namespace BT.Run.Dev
{
    public class RunDevUlti : ExtendedMonoBehaviourRun
    {
        [ContextMenu("SelectBuildStatsObject")]
        public void SelectBuildStatsObject()
        {
#if UNITY_EDITOR
            UnityEditor.Selection.activeGameObject = BuildStats.gameObject;
#endif
        }
    }

}