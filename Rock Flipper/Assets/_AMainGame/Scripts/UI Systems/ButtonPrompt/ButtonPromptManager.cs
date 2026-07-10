using SubjectNerd.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BT.UI.ButtonPrompts
{
    [CreateAssetMenu(fileName = "ButtonPromptManager", menuName = "BSB/SingleInstance/ButtonPromptManager")]
    public class ButtonPromptManager : ScriptableObject
    {    ///
        [SerializeField]
        private List<InputActionAsset> inputActionAssets;

        [Space]
        [SerializeField]
        private MouseKeyboardButtonGlyphTable mouseKeyboardButtonGlyphTable;
        [SerializeField]
        private GamepadButtonGlyphTable xboxButtonGlyphTable;
        [SerializeField]
        private GamepadButtonGlyphTable psButtonGlyphTable;
        [SerializeField]
        private GamepadButtonGlyphTable switchButtonGlyphTable;
        [SerializeField]
        private GamepadButtonGlyphTable steamButtonGlyphTable;
        [SerializeField]
        private GamepadButtonGlyphTable otherGamepadButtonGlyphTable;

        [Space]
        [SerializeField, Reorderable]
        private List<InputActionButtonPromptSprites> promptMap;

        [Header("Editor Onnly---------------------")]
        [Space]
        [SerializeField]
        private List<SpriteMouseKeyboardButtonPair> spriteToMouseKeyboardButtons = new List<SpriteMouseKeyboardButtonPair>();
        [Space]
        [SerializeField]
        private List<SpriteGamepadButtonPair> spriteToGamepadButtons = new List<SpriteGamepadButtonPair>();

        [NonSerialized]
        private bool inited = false;
        [NonSerialized]
        private Dictionary<Guid, ButtonPromptSprites> promptSpritesDictionary;

        [Serializable]
        public struct InputActionButtonPromptSprites
        {
            [HideInInspector]
            public string name;
            [ReadOnly]
            public InputActionReference inputActionReference;

            [Space]
            public MouseKeyboardButton mouseKeyBoardButton;
            public GamepadButton xboxButton;
            public GamepadButton psButton;
            public GamepadButton switchButton;
            public GamepadButton steamButton;
            public GamepadButton otherGamepadButton;

            [Space, ReadOnly]
            public ButtonPromptSprites promptSprites;

            public InputActionButtonPromptSprites UpdateName()
            {
                ///
                var action = inputActionReference.action;
                name = string.Format("[{0}/{1}] {2:00} : {3} - {4}", promptSprites.SetSpriteCount, ButtonPromptSprites.TotalSpritesCount, promptSprites.activeContextOrderId, action.actionMap.name, action.name);

                ///
                return this;
            }

            public InputActionButtonPromptSprites UpdateMouseKeyBoardButton(MouseKeyboardButton mouseKeyBoardButton)
            {
                ///
                this.mouseKeyBoardButton = mouseKeyBoardButton;

                ///
                return this;
            }

            public InputActionButtonPromptSprites UpdateXboxGamepadButton(GamepadButton gamepadButton)
            {
                ///
                xboxButton = gamepadButton;

                ///
                return this;
            }
        }

        public ButtonPromptSprites GetPromptSprites(InputAction inputAction)
        {
#if !UNITY_EDITOR
        ///
        TryInit();

        ///
        return promptSpritesDictionary[inputAction.id]; 
#else
            if (Application.isPlaying)
            {
                ///
                TryInit();

                ///
                return promptSpritesDictionary[inputAction.id];
            }
            else
            {
                ///
                for (int i = 0; i < promptMap.Count; i++)
                {
                    var currentInputAction = promptMap[i].inputActionReference.action;
                    if (inputAction.id == currentInputAction.id)
                    {
                        return promptMap[i].promptSprites;
                    }
                }

                ///
                return new ButtonPromptSprites();
            }
#endif
        }

        private void TryInit()
        {
            ///
            if (inited)
            {
                return;
            }

            ///
            inited = true;

            ///
            promptSpritesDictionary = new Dictionary<Guid, ButtonPromptSprites>();
            foreach (var item in promptMap)
            {
                ///
                if (item.inputActionReference == null)
                {
                    continue;
                }

                ///
                promptSpritesDictionary.Add(item.inputActionReference.action.id, item.promptSprites);
            }
        }

#if UNITY_EDITOR
        [ContextMenu("Editor_Sync")]
        private void Editor_Sync()
        {
            ///
            UnityEditor.Undo.RecordObject(this, "Editor_Sync");
            UnityEditor.EditorUtility.SetDirty(this);

            // Removed missing items
            for (int i = promptMap.Count - 1; i >= 0; i--)
            {
                var item = promptMap[i];

                ///
                if (item.inputActionReference == null)
                {
                    promptMap.RemoveAt(i);
                }
            }

            // Process existing items
            var existedActionIds = new HashSet<Guid>();
            for (int i = 0; i < promptMap.Count; i++)
            {
                ///
                var promptSprites = promptMap[i];

                ///
                existedActionIds.Add(promptSprites.inputActionReference.action.id);

                ///
                var action = promptSprites.inputActionReference.action;
                promptSprites.UpdateName();

                ///
                promptMap[i] = promptSprites;
            }

            // Add new items (if needed)
            List<UnityEngine.Object> allAssets = new List<UnityEngine.Object>();
            foreach (var inputActionAsset in inputActionAssets)
            {
                var assetPath = UnityEditor.AssetDatabase.GetAssetPath(inputActionAsset);
                var assets = UnityEditor.AssetDatabase.LoadAllAssetRepresentationsAtPath(assetPath);
                allAssets.AddRange(assets);
            }
            foreach (var item in allAssets)
            {
                ///
                var inputActionRef = item as InputActionReference;

                ///
                if (inputActionRef == null)
                {
                    return;
                }

                ///
                var action = inputActionRef.action;
                if (existedActionIds.Contains(action.id))
                {
                    continue;
                }

                ///
                var promptSprites = new InputActionButtonPromptSprites()
                {
                    inputActionReference = inputActionRef,
                };
                promptSprites.UpdateName();

                ///
                promptMap.Add(promptSprites);
            }

            // Assign order id
            for (int i = 0; i < promptMap.Count; i++)
            {
                var item = promptMap[i];
                item.promptSprites.activeContextOrderId = promptMap.Count - i;
                promptMap[i] = item;
            }

            // Update sprites
            for (int i = 0; i < promptMap.Count; i++)
            {
                promptMap[i] = Editor_UpdateSprites(promptMap[i]);
            }

            // Update names
            for (int i = 0; i < promptMap.Count; i++)
            {
                promptMap[i] = promptMap[i].UpdateName();
            }
        }

        private InputActionButtonPromptSprites Editor_UpdateSprites(InputActionButtonPromptSprites inputActionButtonPromptSprites)
        {
            ///
            var promptSprites = inputActionButtonPromptSprites.promptSprites;

            // Mouse and keyboard
            promptSprites.mouseAndKeyboardSprite = mouseKeyboardButtonGlyphTable.GetSprite(inputActionButtonPromptSprites.mouseKeyBoardButton);

            // Xbox
            promptSprites.xboxSprite = xboxButtonGlyphTable.GetSprite(inputActionButtonPromptSprites.xboxButton);

            // PS
            var psButton = inputActionButtonPromptSprites.psButton;
            if (psButton == GamepadButton.NotSet)
            {
                psButton = inputActionButtonPromptSprites.xboxButton;
            }
            promptSprites.psSprite = psButtonGlyphTable.GetSprite(psButton);

            // Switch
            var switchButton = inputActionButtonPromptSprites.switchButton;
            if (switchButton == GamepadButton.NotSet)
            {
                switchButton = inputActionButtonPromptSprites.xboxButton;
            }
            promptSprites.switchSprite = switchButtonGlyphTable.GetSprite(switchButton);

            // Steam
            var steamButton = inputActionButtonPromptSprites.steamButton;
            if (steamButton == GamepadButton.NotSet)
            {
                steamButton = inputActionButtonPromptSprites.xboxButton;
            }
            promptSprites.steamSprite = steamButtonGlyphTable.GetSprite(steamButton);

            // other
            var otherGamepadButton = inputActionButtonPromptSprites.otherGamepadButton;
            if (otherGamepadButton == GamepadButton.NotSet)
            {
                otherGamepadButton = inputActionButtonPromptSprites.xboxButton;
            }
            promptSprites.otherGamepadSprite = otherGamepadButtonGlyphTable.GetSprite(otherGamepadButton);

            ///
            inputActionButtonPromptSprites.promptSprites = promptSprites;

            ///
            return inputActionButtonPromptSprites;
        }

        [ContextMenu("UpdateSpriteToMouseKeyboardButtonsList")]
        private void UpdateSpriteToMouseKeyboardButtonsList()
        {
            ///
            HashSet<Sprite> sprites = new HashSet<Sprite>();

            ///
            spriteToMouseKeyboardButtons = new List<SpriteMouseKeyboardButtonPair>();

            ///
            foreach (var item in promptMap)
            {
                ///
                if (item.promptSprites.mouseAndKeyboardSprite == null)
                {
                    continue;
                }

                ///
                if (sprites.Contains(item.promptSprites.mouseAndKeyboardSprite))
                {
                    continue;
                }

                ///
                sprites.Add(item.promptSprites.mouseAndKeyboardSprite);
                spriteToMouseKeyboardButtons.Add(new SpriteMouseKeyboardButtonPair() { Sprite = item.promptSprites.mouseAndKeyboardSprite });
            }

            ///
            UnityEditor.EditorUtility.SetDirty(this);
        }

        [ContextMenu("UpdateSpriteToGamepadButtonsList")]
        private void UpdateSpriteToGamepadButtonsList()
        {
            ///
            HashSet<Sprite> sprites = new HashSet<Sprite>();

            ///
            spriteToGamepadButtons = new List<SpriteGamepadButtonPair>();

            ///
            foreach (var item in promptMap)
            {
                ///
                if (item.promptSprites.xboxSprite == null)
                {
                    continue;
                }

                ///
                if (sprites.Contains(item.promptSprites.xboxSprite))
                {
                    continue;
                }

                ///
                sprites.Add(item.promptSprites.xboxSprite);
                spriteToGamepadButtons.Add(new SpriteGamepadButtonPair() { Sprite = item.promptSprites.xboxSprite });
            }

            ///
            UnityEditor.EditorUtility.SetDirty(this);
        }

        [ContextMenu("UpdateMouseKeyboardButtonForMap")]
        private void UpdateMouseKeyboardButtonForMap()
        {
            Dictionary<Sprite, MouseKeyboardButton> dict = new Dictionary<Sprite, MouseKeyboardButton>();

            foreach (var item in spriteToMouseKeyboardButtons)
            {
                dict.Add(item.Sprite, item.Button);
            }

            ///
            for (int i = 0; i < promptMap.Count; i++)
            {
                if (promptMap[i].promptSprites.mouseAndKeyboardSprite != null)
                {
                    promptMap[i] = promptMap[i].UpdateMouseKeyBoardButton(dict[promptMap[i].promptSprites.mouseAndKeyboardSprite]);
                }
            }

            ///
            UnityEditor.EditorUtility.SetDirty(this);
        }

        [ContextMenu("UpdateXboxGamepadButtonForMap")]
        private void UpdateXboxGamepadButtonForMap()
        {
            Dictionary<Sprite, GamepadButton> dict = new Dictionary<Sprite, GamepadButton>();

            foreach (var item in spriteToGamepadButtons)
            {
                dict.Add(item.Sprite, item.Button);
            }

            ///
            for (int i = 0; i < promptMap.Count; i++)
            {
                if (promptMap[i].promptSprites.mouseAndKeyboardSprite != null)
                {
                    promptMap[i] = promptMap[i].UpdateXboxGamepadButton(dict[promptMap[i].promptSprites.xboxSprite]);
                }
            }

            ///
            UnityEditor.EditorUtility.SetDirty(this);
        }
#endif
    }
}