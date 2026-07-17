using UnityEngine;

namespace Agame.Run
{
    public class SkillNode_Debug : MonoBehaviour
    {
        [SerializeField]
        private UnifiedText text;

        public void SetNode(SkillGraphNode node)
        {
            var s = "";

            ///
            s = node.BuildAgent != null ? node.BuildAgent.name : "<NoAgent>";

            ///
            if (node.BuildAgent != null)
            {
                s += "\r\n" + node.BuildValue;
                //s += ;
            }

            ///
            text.Text = s;
        }
    }

}