using I2.Loc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BT.Localization
{
    public static class LocalizedStringExtensions
    {
        public static bool IsNone(this LocalizedString localizedString)
        {
            return (string.IsNullOrWhiteSpace(localizedString.mTerm)
                || localizedString.mTerm == "-");
        }

        public static bool HasTranslation(this LocalizedString localizedString, string language)
        {
            ///
            LocalizationManager.InitializeIfNeeded();

            ///
            for (int i = 0; i < LocalizationManager.Sources.Count; i++)
            {
                ///
                var source = LocalizationManager.Sources[i];

                ///
                var savedOnMissingTranslation = source.OnMissingTranslation;
                source.OnMissingTranslation = LanguageSourceData.MissingTranslationAction.Empty;

                ///
                var translation = source.GetTranslation(localizedString.mTerm);

                ///
                source.OnMissingTranslation = savedOnMissingTranslation;

                ///
                if (!string.IsNullOrEmpty(translation))
                {
                    return true;
                }
            }

            ///
            return false;
        }

        public static bool HasTranslationForCurrentLanguage(this LocalizedString localizedString)
        {
            return localizedString.HasTranslation(LocalizationManager.CurrentLanguage);
        }
    }

}