using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BT.UI.ButtonPrompts
{
    public class ButtonGryphTable<T, U> : ScriptableObject where T : ISpriteButtonPair<T, U>, new()
    {
        [SerializeField]
        private List<T> spriteButtonPairs;

        [System.NonSerialized]
        private Dictionary<U, Sprite> dictionary = null;

        public Sprite GetSprite(U button)
        {
            ///
            if (dictionary == null)
            {
                BuildDictionary();
            }

            ///
            return dictionary[button];
        }

        private void BuildDictionary()
        {
            ///
            dictionary = new Dictionary<U, Sprite>();

            ///
            foreach (var pair in spriteButtonPairs)
            {
                dictionary.Add(pair.Button, pair.Sprite);
            }
        }

#if UNITY_EDITOR
        [ContextMenu("Editor_Sync")]
        private void Editor_Sync()
        {
            ///
            if (spriteButtonPairs == null)
            {
                spriteButtonPairs = new List<T>();
            }

            ///
            HashSet<U> buttons = new HashSet<U>();

            ///
            foreach (T item in spriteButtonPairs)
            {
                buttons.Add(item.Button);
            }

            ///
            foreach (U button in Enum.GetValues(typeof(U)))
            {
                if (buttons.Add(button))
                {
                    var pair = new T() { Button = button, Sprite = null };
                    spriteButtonPairs.Add(pair);
                }
            }

            ///
            for (int i = 0; i < spriteButtonPairs.Count; i++)
            {
                spriteButtonPairs[i] = spriteButtonPairs[i].UpdateName();
            }

            ///
            UnityEditor.EditorUtility.SetDirty(this);
        }
#endif
    }

}