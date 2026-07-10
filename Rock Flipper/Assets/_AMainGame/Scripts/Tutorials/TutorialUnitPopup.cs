using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Agame.Tutorials
{
    public class TutorialUnitPopup : TutorialUnit
    {
        protected override void Show()
        {
            StartCoroutine(ShowPopupWhenMainScreenActive(viewWrapper));
        }
        protected IEnumerator ShowPopupWhenMainScreenActive(GameObject popup)
        {
            ///
            while (!CommonEntry.mainUIScreen.IsScreenActive)
            {
                yield return null;
            }

            ///
            popup.SetActive(true);
        }
    }

}