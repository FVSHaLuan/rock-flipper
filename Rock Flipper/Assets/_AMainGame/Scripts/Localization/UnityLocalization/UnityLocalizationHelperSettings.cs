using UnityEngine;
using UnityEngine.Localization;

namespace Agame.Localization
{
    public class UnityLocalizationHelperSettings : ScriptableObjectWithInit
    {
        private const string SettingsFilePath = "Assets/_AMainGame/Data/Localization/UnityLocalizationHelperSettings.asset";

        private static UnityLocalizationHelperSettings instance;

        [SerializeField]
        private LocalizedTmpFont defaultLocalizedTmpFont;
        [SerializeField]
        private LocalizedMaterial defaultLocalizedTmpFontMaterial;        

        public LocalizedTmpFont DefaultLocalizedTmpFont => defaultLocalizedTmpFont;
        public LocalizedMaterial DefaultLocalizedTmpFontMaterial => defaultLocalizedTmpFontMaterial;

        public static UnityLocalizationHelperSettings Instance
        {
            get
            {
#if UNITY_EDITOR
                if (instance == null)
                {
                    instance = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityLocalizationHelperSettings>(SettingsFilePath);
                    if (instance == null)
                    {
                        instance = CreateInstance<UnityLocalizationHelperSettings>();
                        UnityEditor.AssetDatabase.CreateAsset(instance, SettingsFilePath);
                        UnityEditor.AssetDatabase.SaveAssets();
                        UnityEditor.AssetDatabase.Refresh();
                    }
                }
#endif

                ///
                return instance;
            }
        }
    }

}