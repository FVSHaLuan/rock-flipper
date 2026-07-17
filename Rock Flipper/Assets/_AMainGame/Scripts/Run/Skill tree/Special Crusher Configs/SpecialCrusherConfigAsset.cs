using I2.Loc;
using UnityEngine;

namespace Agame.Run
{
    public class SpecialCrusherConfigAsset : ScriptableObjectWithInit
    {
        [SerializeField]
        private SpecialCrusherId specialCrusherId;
        [SerializeField]
        private string specialCrusherName;
        [SerializeField]
        private LocalizedString specialCrusherNameLocalized;
        [SerializeField]
        private Sprite icon;
        [SerializeField, TextArea]
        private string specialCrusherDescription;
        [SerializeField]
        private LocalizedString specialCrusherDescriptionLocalized;

        public string SpecialCrusherName => specialCrusherNameLocalized;
        public SpecialCrusherId SpecialCrusherId => specialCrusherId;
        public Sprite Icon => icon;
        public string SpecialCrusherDescription => specialCrusherDescriptionLocalized;
    }

}