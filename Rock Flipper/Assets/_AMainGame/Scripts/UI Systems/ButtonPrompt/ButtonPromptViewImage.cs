using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Agame.UI.ButtonPrompts
{
    [RequireComponent(typeof(Image))]
    public class ButtonPromptViewImage : ButtonPromptView
    {
        [SerializeField]
        private Image secondaryImage;

        [Space]
        [SerializeField]
        private UnityEvent onFoundSprite;
        [SerializeField]
        private UnityEvent onNotFoundSprite;

        private Image image;

        protected override bool Init()
        {
            ///
            image = GetComponent<Image>();

            ///
            return base.Init();
        }

        protected override void UpdateView(Sprite sprite)
        {
            ///
            image.sprite = sprite;
            image.enabled = sprite != null;

            ///
            if (secondaryImage != null)
            {
                secondaryImage.sprite = sprite;
                secondaryImage.enabled = sprite != null;
            }

            ///
            if (sprite == null)
            {
                onNotFoundSprite?.Invoke();
            }
            else
            {
                onFoundSprite?.Invoke();
            }
        }
    }
}