using FH.Core.Architecture.Pool;
using UnityEngine;

namespace Agame.Run.Combat
{
    [RequireComponent(typeof(Chest))]
    public class ChestPoolHandler : GeneralPoolMemberSimplifiedHandler<Chest>
    {
        [SerializeField]
        private Chest chest;

        public override Chest TargetObject => chest;

#if UNITY_EDITOR
        protected void Reset()
        {
            chest = GetComponent<Chest>();
        }
#endif
    }

}