using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BT.Localization.Dev
{
    public interface ILocalizable
    {
        public bool Editor_IsLocalized { get; }

        public void Editor_TryCreatingLocalizationTerms(object context)
        {
            // Intentionally left blank
        }
    }

}