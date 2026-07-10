using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Agame.UI.ButtonPrompts
{
    public interface ISpriteButtonPair<T, U> where T : ISpriteButtonPair<T, U>
    {
        Sprite Sprite { get; set; }
        U Button { get; set; }
        T UpdateName();
    }

    [Serializable]
    public struct SpriteGamepadButtonPair : ISpriteButtonPair<SpriteGamepadButtonPair, GamepadButton>
    {
        [SerializeField]
        private string name;
        [SerializeField]
        private Sprite sprite;
        [SerializeField]
        private GamepadButton gamepadButton;

        public Sprite Sprite { get => sprite; set => sprite = value; }
        public GamepadButton Button { get => gamepadButton; set => gamepadButton = value; }

        public SpriteGamepadButtonPair UpdateName()
        {
            ///
            name = string.Format("{0}{1}", Sprite == null ? "[NULL]" : null, Button);

            ///
            return this;
        }
    }

    [Serializable]
    public struct SpriteMouseKeyboardButtonPair : ISpriteButtonPair<SpriteMouseKeyboardButtonPair, MouseKeyboardButton>
    {
        [SerializeField]
        private string name;
        [SerializeField]
        private MouseKeyboardButton mouseKeyBoardButton;
        [SerializeField]
        private Sprite sprite;

        public Sprite Sprite { get => sprite; set => sprite = value; }
        public MouseKeyboardButton Button { get => mouseKeyBoardButton; set => mouseKeyBoardButton = value; }

        public SpriteMouseKeyboardButtonPair UpdateName()
        {
            ///
            name = string.Format("{0}{1}", Sprite == null ? "[NULL]" : null, Button);

            ///
            return this;
        }
    }
}