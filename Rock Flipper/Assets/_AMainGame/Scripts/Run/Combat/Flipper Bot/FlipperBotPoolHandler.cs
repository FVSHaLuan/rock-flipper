using FH.Core.Architecture.Pool;
using UnityEngine;

namespace Agame.Run.Combat
{
    [RequireComponent(typeof(FlipperBot))]
    public class FlipperBotPoolHandler : GeneralPoolMemberSimplifiedHandler<FlipperBot>
    {
        [SerializeField]
        private FlipperBot flipperBot;

        public override FlipperBot TargetObject => flipperBot;

        protected void Reset()
        {
            flipperBot = GetComponent<FlipperBot>();
        }
    }

}