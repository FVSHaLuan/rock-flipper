using Agame.Run.Stats;
using System.Collections.Generic;
using UnityEngine;

namespace Agame.Run.Shop
{
    public class SideBarStatBuilder : MonoBehaviourWithInit
    {
        private List<IStatBuilder> statBuilders = new List<IStatBuilder>();

        protected override bool Init()
        {
            ///
            GetComponentsInChildren(true, statBuilders);

            ///
            return base.Init();
        }

        public void Apply()
        {
            ///
            TryInit();

            ///
            foreach (var statBuilder in statBuilders)
            {
                statBuilder.ApplyToBuildStats();
            }
        }
    }

}