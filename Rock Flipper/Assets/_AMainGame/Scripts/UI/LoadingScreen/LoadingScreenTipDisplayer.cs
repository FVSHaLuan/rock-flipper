using Agame.UI.Tips;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Agame.UI.LoadingScreen
{
    public class LoadingScreenTipDisplayer : MonoBehaviour
    {
        [SerializeField]
        private GameTipManager gameTipManager;

        [Space]
        [SerializeField]
        private Transform tipRoot;

        protected void Start()
        {
            ///
            GameTip gameTip = gameTipManager.GetRandomGameTip();

            ///
            while (!gameTip.ShowAtLoadingScreen)
            {
                gameTip = gameTipManager.GetRandomGameTip();
            }

            ///
            Instantiate(gameTip, tipRoot, false);

            ///
            gameTip.transform.localScale = Vector3.one;
            gameTip.transform.localPosition = Vector3.zero;
        }
    }

}