using UnityEngine;
using XNode;

namespace Agame.Run
{
    [NodeTint(255, 0, 0)]
    public class TrueBoolNode : Node
    {
        [SerializeField, Output(connectionType = ConnectionType.Override)]
        private bool value = true;

        public override object GetValue(NodePort port)
        {
            return true;
        }
    }

}