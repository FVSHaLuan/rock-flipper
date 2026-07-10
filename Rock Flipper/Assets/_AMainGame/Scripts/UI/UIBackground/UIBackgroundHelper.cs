using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BT.UI
{
    public class UIBackgroundHelper : MonoBehaviour
    {
        [SerializeField]
        private Transform popupTransform;
        [SerializeField]
        private bool isAbsolute;
        [SerializeField]
        private bool forceNotShow;

        UIScreen uiScreen;

        public bool ForceNotShow
        {
            get => forceNotShow;
            set
            {
                forceNotShow = value;
                if (forceNotShow)
                {
                    RemoveShowingLock();
                }
                else
                {
                    if (uiScreen == null || uiScreen.IsScreenActive)
                    {
                        AddShowingLock();
                    }
                }
            }
        }

        protected void OnDestroy()
        {
            ///
            if (uiScreen != null)
            {
                ///
                uiScreen.OnBecomeActive -= UiScreen_OnBecomeActive;
                uiScreen.OnBecomeInactive -= UiScreen_OnBecomeInactive;
            }
        }

        protected void Awake()
        {
            ///
            uiScreen = popupTransform.GetComponent<UIScreen>();

            ///
            if (uiScreen != null)
            {
                ///
                uiScreen.OnBecomeActive += UiScreen_OnBecomeActive;
                uiScreen.OnBecomeInactive += UiScreen_OnBecomeInactive;

                ///
                if (uiScreen.IsScreenActive && !forceNotShow)
                {
                    UiScreen_OnBecomeActive();
                }
            }
        }

        protected void OnEnable()
        {
            if (uiScreen == null && !forceNotShow)
            {
                AddShowingLock();
            }
        }

        protected void OnDisable()
        {
            if (uiScreen == null)
            {
                RemoveShowingLock();
            }
        }

        private void UiScreen_OnBecomeInactive()
        {
            RemoveShowingLock();
        }

        private void UiScreen_OnBecomeActive()
        {
            if (isActiveAndEnabled && !forceNotShow)
            {
                AddShowingLock();
            }
        }

        [ContextMenu("AddShowingLock"), PlayModeOnly]
        public void AddShowingLock()
        {
            if (!isAbsolute)
            {
                Entry.Instance.uiBackgroundManager.AddShowingLock(this, popupTransform);
            }
            else
            {
                Entry.Instance.absoluteUIBackgroundManager.AddShowingLock(this, popupTransform);
            }
        }

        [ContextMenu("RemoveShowingLock"), PlayModeOnly]
        public void RemoveShowingLock()
        {
            if (!isAbsolute)
            {
                Entry.Instance.uiBackgroundManager.RemoveShowingLock(this, popupTransform);
            }
            else
            {
                Entry.Instance.absoluteUIBackgroundManager.RemoveShowingLock(this, popupTransform);
            }
        }

#if UNITY_EDITOR
        [ContextMenu("Editor_SetUIScreenAsPopupTransform")]
        private void Editor_SetUIScreenAsPopupTransform()
        {
            ///
            UnityEditor.Undo.RecordObject(this, "Editor_SetUIScreenAsPopupTransform");

            ///
            var uiScreen = GetComponentInParent<UIScreen>(true);
            popupTransform = uiScreen?.GetComponent<RectTransform>();
        }
#endif
    }

}