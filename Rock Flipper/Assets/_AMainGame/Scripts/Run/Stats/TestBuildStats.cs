using System.Collections.Generic;
using UnityEngine;

namespace BT.Run.Stats
{
    public class TestBuildStats : ScriptableObjectWithInit
    {
        [SerializeField]
        private bool enabled;
        [SerializeField, Tooltip("Using the first one")]
        private List<BuildStatsObject> buildStatsObjects = new List<BuildStatsObject>();

        public bool Enabled => enabled && (buildStatsObjects != null && buildStatsObjects.Count > 0);
        public BuildStatsObject BuildStats => buildStatsObjects[0];
    }

}