using I2.Loc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BT.Localization
{
    public static class I2LocalizationExtensions
    {
#if UNITY_EDITOR
        public static bool AddTextTermIfNotExisted(this LanguageSourceAsset languageSourceAsset, string term, string englishText)
        {
            ///
            if (languageSourceAsset.SourceData.ContainsTerm(term))
            {
                return false;
            }

            ///
            var termData = languageSourceAsset.SourceData.AddTerm(term);

            ///
            termData.SetTranslation(0, englishText);

            ///
            languageSourceAsset.SourceData.UpdateDictionary();

            ///
            UnityEditor.EditorUtility.SetDirty(languageSourceAsset);

            ///
            return true;
        }
#endif    

        public static string ToEnglishString(this LocalizedString localizedString)
        {
            var translation = LocalizationManager.GetTranslation
                (Term: localizedString.mTerm,
                 FixForRTL: !localizedString.mRTL_IgnoreArabicFix,
                 maxLineLengthForRTL: localizedString.mRTL_MaxLineLength,
                 ignoreRTLnumbers: !localizedString.mRTL_ConvertNumbers,
                 allowLocalizedParameters: false,
                 localParametersRoot: null, overrideLanguage: "english");

            ///
            LocalizationManager.ApplyLocalizationParams(ref translation, !localizedString.m_DontLocalizeParameters);
            
            ///
            return translation;
        }
    }
}