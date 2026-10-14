using UnityEngine;

namespace Agame
{
    /// <summary>
    /// Items the player hasn't seen since they were found or leveled up (<see cref="Run.Shop.ItemState.unseen"/>).
    /// </summary>
    public partial class RunData
    {
        public bool HasUnseenItems
        {
            get
            {
                ///
                if (itemStates == null)
                {
                    return false;
                }

                ///
                foreach (var itemState in itemStates.Values)
                {
                    if (itemState.unseen)
                    {
                        return true;
                    }
                }
                return false;
            }
        }
    }

}
