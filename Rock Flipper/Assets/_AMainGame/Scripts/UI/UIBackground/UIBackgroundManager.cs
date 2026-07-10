using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Agame.UI
{
    public class UIBackgroundManager : ExtendedMonoBehaviour
    {
        public event UIBackground.EventHandler OnShowed
        {
            add
            {
                ///
                TryGetUIBackground();

                ///
                activeBackground.OnShowed += value;
            }

            remove
            {
                ///
                activeBackground.OnShowed -= value;

            }
        }

        public event UIBackground.EventHandler OnHid
        {
            add
            {
                ///
                TryGetUIBackground();

                ///
                activeBackground.OnHid += value;
            }

            remove
            {
                ///
                activeBackground.OnHid -= value;
            }
        }

        [SerializeField]
        private UIBackground backgroundPrototype;

        private UIBackground activeBackground;

        public UIBackground ActiveBackground => activeBackground;

        public void AddShowingLock(object @object, Transform transform)
        {
            ///
            TryGetUIBackground();

            ///
            activeBackground.AddShowingLock(@object, transform);
        }

        private void TryGetUIBackground()
        {
            if (activeBackground == null)
            {
                activeBackground = generalPool.TakeInstance(backgroundPrototype, this);
                activeBackground.gameObject.SetActive(true);
            }
        }

        public void RemoveShowingLock(object @object, Transform transform)
        {
            ///
            if (activeBackground == null)
            {
                return;
            }

            ///
            activeBackground.RemoveShowingLock(@object, transform);
        }
    }

}