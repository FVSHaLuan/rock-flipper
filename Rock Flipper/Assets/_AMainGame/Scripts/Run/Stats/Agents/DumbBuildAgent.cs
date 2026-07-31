using UnityEngine;

namespace Agame.Run.Stats.Agents
{
    public class DumbBuildAgent : BuildAgent
    {
        public override void Apply(int currentLevel, int addingLevel, double buildValuePerLevel)
        {
            Debug.Log($"DumbBuildAgent.Apply called with currentLevel: {currentLevel}, addingLevel: {addingLevel}, buildValuePerLevel: {buildValuePerLevel}");
        }
    }

}