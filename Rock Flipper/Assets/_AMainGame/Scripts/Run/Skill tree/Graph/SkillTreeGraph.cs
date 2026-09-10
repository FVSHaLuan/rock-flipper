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
        [SerializeField]
        private Color rootNodeHeaderColor= new Color(227f/255f, 75f/255f, 93f/255f);

        public SkillGraphNode RootNode => rootNode;
        public Color RootNodeHeaderColor => rootNodeHeaderColor;
        public int CompactModeWidth => compactModeWidth;
    }

}