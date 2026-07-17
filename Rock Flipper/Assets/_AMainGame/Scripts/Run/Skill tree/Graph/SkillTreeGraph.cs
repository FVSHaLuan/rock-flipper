using UnityEngine;
using XNode;

namespace Agame.Run
{
    public class SkillTreeGraph : NodeGraph
    {
        [Header("SkillTreeGraph")]
        [SerializeField]
        private SkillGraphNode rootNode;

        [Header("Draw settings")]       
        [SerializeField]
        private int compactModeWidth = 100;

        public SkillGraphNode RootNode => rootNode;
        public int CompactModeWidth => compactModeWidth;
    }

}