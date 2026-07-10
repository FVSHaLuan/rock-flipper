using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Agame.Home
{
    public class HomeEntry : CommonEntry
    {
        public static HomeEntry Instance { get; private set; }

        [Header("HomeEntry")]
        public Camera captureCamera;

        protected override void ExtendedAwake()
        {
            ///
            base.ExtendedAwake();

            ///
            Instance = this;
            CommonEntry.CommonInstance = this;
            Entry.ActiveGameScene = GameScene.Home;
        }
    }

}