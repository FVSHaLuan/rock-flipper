using Agame.Run.Shop;
using UnityEngine;

namespace Agame.Run.Stats
{
    public abstract class StatBuilderButtonSpecificMonoBehaviour : ExtendedMonoBehaviourRun, IStatBuilderButtonSpecific
    {
        public virtual void HandleLeveledUp(int levelCount)
        {
            // intentionally left blank, to be overridden by subclasses
        }
    }

}