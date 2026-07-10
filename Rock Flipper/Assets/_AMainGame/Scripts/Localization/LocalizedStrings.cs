using I2.Loc;
using System.Collections;
using System.Collections.Generic;
using Agame.Dev;
using UnityEngine;

namespace Agame.Localization
{
    public class LocalizedStrings : ScriptableObjectWithInit
    {
        [SerializeField]
        private LocalizedString secondChance;

        public static LocalizedStrings Instance
        {
            get
            {
                ///
#if UNITY_EDITOR
                if (!Application.isPlaying)
                {
                    return DevEntry.Instance.localizedStrings;
                }
#endif

                ///
                return Entry.Instance.localizedStrings;
            }
        }

        public static string SecondChance => Instance.secondChance;
    }

}