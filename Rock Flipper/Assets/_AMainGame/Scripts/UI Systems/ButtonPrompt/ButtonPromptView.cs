using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BT.UI.ButtonPrompts
{
    public abstract class ButtonPromptView : ExtendedMonoBehaviour
    {
        [SerializeField]
        private InputActionReference inputAction;

        public InputActionReference InputAction
        {
            set
            {
                ///
                TryInit();

                ///
                inputAction = value;
                UpdateView();
            }
        }

        protected abstract void UpdateView(Sprite sprite);

        public void OnEnable()
        {
            ///
            UpdateView();

            ///
            entry.inputManager.OnActiveInputDeviceChanged += InputManager_OnActiveInputDeviceChanged;
        }

        protected void OnDisable()
        {
            ///
            entry.inputManager.OnActiveInputDeviceChanged -= InputManager_OnActiveInputDeviceChanged;
        }

        private void InputManager_OnActiveInputDeviceChanged()
        {
            UpdateView();
        }

        private void UpdateView()
        {
            ///
            if (inputAction == null)
            {
                return;
            }

            ///
            var simplifiedInputDevice = entry.inputManager.ActiveSimplifiedInputDevice;
            var sprite = GetSprite(simplifiedInputDevice);

            ///
            UpdateView(sprite);
        }

        private ButtonPromptSprites GetPromptSprites()
        {
            return entry.buttonPromptManager.GetPromptSprites(inputAction.action);
        }

        private Sprite GetSprite(SimplifiedInputDevice simplifiedInputDevice)
        {
            ///
            var promptSprites = GetPromptSprites();

            ///
            switch (simplifiedInputDevice.deviceType)
            {
                case SimplifiedInputDeviceType.MouseAndKeyboard:
                    return promptSprites.mouseAndKeyboardSprite;
                case SimplifiedInputDeviceType.Gamepad:
                    switch (simplifiedInputDevice.gamepadType)
                    {
                        case SimplifiedGamepadType.Xbox:
                            return promptSprites.xboxSprite;
                        case SimplifiedGamepadType.PS:
                            return promptSprites.psSprite;
                        case SimplifiedGamepadType.Switch:
                            return promptSprites.switchSprite;
                        case SimplifiedGamepadType.Other:
                        case SimplifiedGamepadType.None:
                            return promptSprites.otherGamepadSprite;
                        default:
                            return promptSprites.otherGamepadSprite;
                    }
                default:
                    return promptSprites.mouseAndKeyboardSprite;
            }
        }
    }
}