using FH.Core.Architecture.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BSB.UISystems
{
    public class MiniConsoleItem : GeneralPoolMemberSimplified
    {
        [SerializeField]
        private UnifiedText text;
        [SerializeField]
        private Image image;
        [SerializeField]
        private Color defaultIconColor = Color.white;

        private MiniConsoleItemData data;

        public void SetData(MiniConsoleItemData data)
        {
            ///
            this.data = data;

            ///
            text.Text = data.text;
            if (image != null)
            {
                if (data.icon != null)
                {
                    ///
                    image.gameObject.SetActive(true);
                    image.sprite = data.icon;

                    ///                    
                    image.color = data.setIconColor ? data.iconColor : defaultIconColor;
                }
                else
                {
                    image.gameObject.SetActive(false);
                }
            }
        }
    }
}
