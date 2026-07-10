using I2.Loc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Agame.Localization
{
    public class LocalizedFeature : ExtendedMonoBehaviour
    {
        [SerializeField]
        private LocalizedString localizedString;

        protected void OnEnable()
        {
            if (!localizedString.HasTranslationForCurrentLanguage())
            {
                gameObject.SetActive(false);
            }
        }
    }

}