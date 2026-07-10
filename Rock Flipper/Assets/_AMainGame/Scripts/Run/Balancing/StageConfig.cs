using GD;
using System.Collections.Generic;
using UnityEngine;

namespace BT.Balancing
{
    public class StageConfig : ScriptableObjectWithInit
    {
        [SerializeField]
        private double startMaxHp = 2;

        [SerializeField]
        private int editor_TestStage = 10;

        [Space]
        [SerializeField]
        private List<float> multipliers;

        [System.NonSerialized]
        private List<double> coreMaxHps = new List<double>();

        public double GetCoreHPByStage(int stage)
        {
            ///
            if (coreMaxHps == null)
            {
                coreMaxHps = new List<double>();
            }

            ///
            while (coreMaxHps.Count <= stage)
            {
                if (coreMaxHps.Count == 0)
                {
                    coreMaxHps.Add(startMaxHp);
                }
                else
                {
                    var currentIndex = coreMaxHps.Count - 1;
                    var currentHp = coreMaxHps[currentIndex];
                    var currentMultiplier = multipliers.GetItemWithClampedIndex(currentIndex);
                    var nextHp = currentHp * currentMultiplier;
                    coreMaxHps.Add(nextHp);
                }
            }

            ///
            return coreMaxHps[stage];
        }

#if UNITY_EDITOR
        [ContextMenu("Editor_ClearHps")]
        private void Editor_ClearHps()
        {
            coreMaxHps.Clear();
        }

        [ContextMenu("Editor_ShowMaxHPForTestStage")]
        private void Editor_ShowMaxHPForTestStage()
        {
            if (coreMaxHps != null)
            {
                coreMaxHps.Clear();
            }

            ///
            Debug.Log($"Max HP for test stage {editor_TestStage}: {GetCoreHPByStage(editor_TestStage)}");
        }
#endif
    }

}