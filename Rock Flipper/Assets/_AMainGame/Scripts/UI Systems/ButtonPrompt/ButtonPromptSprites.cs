using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Agame.UI.ButtonPrompts
{
    [System.Serializable]
    public struct ButtonPromptSprites
    {
        public const int TotalSpritesCount = 5;

        public bool isHold;
        public int activeContextOrderId;

        [Space]
        public Sprite mouseAndKeyboardSprite;
        public Sprite xboxSprite;
        public Sprite psSprite;
        public Sprite switchSprite;
        public Sprite steamSprite;
        public Sprite otherGamepadSprite;

        public int SetSpriteCount
        {
            get
            {
                ///
                int count = 0;

                ///
                count += mouseAndKeyboardSprite == null ? 0 : 1;
                count += xboxSprite == null ? 0 : 1;
                count += psSprite == null ? 0 : 1;
                count += switchSprite == null ? 0 : 1;
                count += otherGamepadSprite == null ? 0 : 1;

                ///
                return count;
            }
        }
    }
}