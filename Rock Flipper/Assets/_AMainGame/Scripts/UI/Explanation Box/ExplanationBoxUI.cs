using FH.Core.Architecture.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BT.UI
{
    public class ExplanationBoxUI : GeneralPoolMemberSimplified
    {
        [SerializeField]
        private UnifiedText explanationText;

        public string Text
        {
            set
            {
                ///
                explanationText.Text = value;
            }
        }
    }

}