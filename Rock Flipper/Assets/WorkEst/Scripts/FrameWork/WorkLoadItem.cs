using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WorkEst
{
    [CreateAssetMenu(fileName = "WorkLoadItem", menuName = "WorkEst/WorkLoadItem")]
    public class WorkLoadItem : WorkLoadBase
    {
        [SerializeField, Min(0)]
        private float time;

        protected override float GetTime(HashSet<WorkLoadBase> calculatedWorks)
        {
            return time;
        }
    }
}
