using UnityEngine;

namespace BT.Run
{
    public class RunAutoSaver : ExtendedMonoBehaviourRun
    {
        [SerializeField]
        private float autoSaveInterval = 60f;
        [SerializeField]
        private float editorAutoSaveInterval = 10f;

        protected void Update()
        {
            /// 
            var interval = Application.isEditor ? editorAutoSaveInterval : autoSaveInterval;

            ///
            var timePassed = Time.realtimeSinceStartup - RunData.LastSaveTime;
            if (timePassed >= interval)
            {
                entry.playerDataSaver.SetSaveThisFrame();
            }
        }
    }

}