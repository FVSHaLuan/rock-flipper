using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WorkEst
{
    public abstract class WorkLoadBase : ScriptableObject
    {
        private const string Label = "WorkestData";

        [System.NonSerialized]
        private bool hasCalculatedTime = false;
        [System.NonSerialized]
        private float time;

        [System.NonSerialized]
        private string workName;

        public bool HasCalculatedTime => hasCalculatedTime;
        public string WorkName
        {
            get
            {
#if UNITY_EDITOR
                ///
                if (string.IsNullOrWhiteSpace(workName))
                {
                    workName = UnityEditor.ObjectNames.NicifyVariableName(name);
                }
#endif

                ///
                return workName;
            }
        }
        public float Time
        {
            get
            {
                if (hasCalculatedTime)
                {
                    return time;
                }
                else
                {
                    throw new System.Exception("Time has been not calculated");
                }
            }
        }

        public void CalculateTime(HashSet<WorkLoadBase> calculatedWorks)
        {
            ///
            if (calculatedWorks.Contains(this))
            {
                throw new System.Exception(string.Format("WorkLoad included more than once, name = {0}", WorkName));
            }

            ///
            calculatedWorks.Add(this);

            ///
            time = GetTime(calculatedWorks);

            ///
            if (time <= 0)
            {
                Debug.LogWarning(string.Format("WorkLoad has <= 0 time, name = {0}", WorkName), this);
            }

            ///
            hasCalculatedTime = true;
        }

        protected abstract float GetTime(HashSet<WorkLoadBase> calculatedWorks);

#if UNITY_EDITOR
        [ContextMenu("TestTimeCalculation")]
        private void TestTimeCalculation()
        {
            ///
            CalculateTime(new HashSet<WorkLoadBase>());

            ///
            Debug.LogFormat("Done calculating for {0}, time = {1} ({2}) - {3}", name, time.ToWorkTimeString(), time, (DateTime.Now + TimeSpan.FromDays(time)).ToString("MMM dd yyyy"));
        }

        [ContextMenu("Auto Name")]
        private void UseFileNameAsWorkName()
        {
            ///
            workName = UnityEditor.ObjectNames.NicifyVariableName(this.name);

            ///
            UnityEditor.EditorUtility.SetDirty(this);
        }

        public void SetLabel()
        {
            var labels = new List<string>(UnityEditor.AssetDatabase.GetLabels(this));
            if (!labels.Contains(Label))
            {
                labels.Add(Label);
            }

            ///
            UnityEditor.AssetDatabase.SetLabels(this, labels.ToArray());
        }
#endif
    }
}