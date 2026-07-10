using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FH.Core.Architecture.Pool
{
    public class GeneralPoolMemberSimplifiedSprite : GeneralPoolMemberSimplified
    {
        [SerializeField]
        private SpriteRenderer spriteRenderer;

        public void SetSprite(Sprite sprite)
        {
            spriteRenderer.sprite = sprite;
        }

        public void SetSpriteScale(Vector3 scale)
        {
            spriteRenderer.transform.localScale = scale;
        }
    }

}