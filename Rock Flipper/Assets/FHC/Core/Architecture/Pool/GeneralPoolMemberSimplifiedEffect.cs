using UnityEngine;

namespace FH.Core.Architecture.Pool
{
    public class GeneralPoolMemberSimplifiedEffect : GeneralPoolMemberSimplifiedSprite
    {
        [SerializeField]
        private UnifiedText text;

        public void SetText(string text)
        {
            this.text.Text = text;
        }
    }
}
