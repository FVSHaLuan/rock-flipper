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
        private Sprite icon;
        [SerializeField, TextArea]
        private string specialCrusherDescription;

        public string SpecialCrusherName => specialCrusherName;
        public SpecialCrusherId SpecialCrusherId => specialCrusherId;
        public Sprite Icon => icon;
        public string SpecialCrusherDescription => specialCrusherDescription;
    }

}