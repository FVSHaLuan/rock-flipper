using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Agame.UI
{
    public class AlwaysOnTopOfUIBackground : ExtendedMonoBehaviour
    {
        [SerializeField]
        private Transform defaultTransformParent;

        protected void OnEnable()
        {
            ///
            entry.uiBackgroundManager.OnShowed += UiBackgroundManager_OnShowed;
            entry.uiBackgroundManager.OnHid += UiBackgroundManager_OnHid;

            ///
            var uiBackground = entry.uiBackgroundManager.ActiveBackground;
            if (uiBackground != null && uiBackground.IsShowing)
            {
                SetOnTopOf(uiBackground);
            }
        }

        protected void OnDisable()
        {
            entry.uiBackgroundManager.OnShowed -= UiBackgroundManager_OnShowed;
            entry.uiBackgroundManager.OnHid -= UiBackgroundManager_OnHid;
        }

        private void UiBackgroundManager_OnHid(UIBackground uiBackground)
        {
            transform.SetParent(defaultTransformParent);
            transform.SetAsFirstSibling();
        }

        private void UiBackgroundManager_OnShowed(UIBackground uiBackground)
        {
            SetOnTopOf(uiBackground);
        }

        private void SetOnTopOf(UIBackground uiBackground)
        {
            transform.SetParent(uiBackground.ChildRoot);
            transform.SetAsFirstSibling();
        }

#if UNITY_EDITOR
        protected void Reset()
        {
            defaultTransformParent = transform.parent;
        }
#endif
    }

}