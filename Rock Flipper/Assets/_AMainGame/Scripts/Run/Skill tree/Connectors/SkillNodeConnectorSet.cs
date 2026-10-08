using UnityEngine;

namespace Agame.Run
{
    public class SkillNodeConnectorSet : MonoBehaviour
    {
        private SkillNodeConnector[] connectorsByDirection;

        public SkillNodeConnector GetConnector(Direction8 direction)
        {
            if (connectorsByDirection == null)
            {
                CacheConnectors();
            }

            return connectorsByDirection[(int)direction];
        }

        private void CacheConnectors()
        {
            connectorsByDirection = new SkillNodeConnector[8];

            foreach (var connector in GetComponentsInChildren<SkillNodeConnector>(true))
            {
                int index = (int)connector.Direction;
                if (connectorsByDirection[index] != null)
                {
                    Debug.LogError($"{name}: more than one {nameof(SkillNodeConnector)} for direction {connector.Direction}", this);
                    continue;
                }

                connectorsByDirection[index] = connector;
            }
        }
    }

}
