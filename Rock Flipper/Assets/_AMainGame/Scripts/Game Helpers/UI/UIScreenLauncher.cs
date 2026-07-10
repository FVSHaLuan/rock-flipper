using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace BT.GameplayHelpers
{
    [DisallowMultipleComponent]
    public class UIScreenLauncher : ExtendedMonoBehaviour
    {
        [SerializeField]
        private UIScreen uiScreen;

        [Space]
        [SerializeField]
        private UnityEvent onBecomeActive;
        [SerializeField]
        private UnityEvent onBecomeInactive;
        [SerializeField]
        private UnityEvent onPoppedFromStack;

        private UIScreen currentUIScreen;

        protected void OnDisable()
        {
            UnlaunchCurrentUIScreen();
        }

        [ContextMenu("Launch")]
        public void Launch()
        {
            Launch(uiScreen);
        }

        private void Launch(UIScreen uiScreen)
        {
            ///
            UnlaunchCurrentUIScreen();

            ///
            currentUIScreen = uiScreen;

            ///
            currentUIScreen.OnBecomeActive += UiScreen_OnBecomeActive;
            currentUIScreen.OnBecomeInactive += UiScreen_OnBecomeInactive;

            ///
            entry.uiScreenManager.OnUIScreenPoppedFromStack += UiScreenManager_OnUIScreenPoppedFromStack;

            ///
            currentUIScreen.enabled = true;
            currentUIScreen.gameObject.SetActive(true);
        }

        private void UiScreen_OnBecomeInactive()
        {
            onBecomeInactive.Invoke();
        }

        private void UiScreen_OnBecomeActive()
        {
            onBecomeActive.Invoke();
        }

        private void UnlaunchCurrentUIScreen()
        {
            ///
            if (currentUIScreen != null)
            {
                currentUIScreen.OnBecomeActive -= UiScreen_OnBecomeActive;
                currentUIScreen.OnBecomeInactive -= UiScreen_OnBecomeInactive;
            }

            ///
            entry.uiScreenManager.OnUIScreenPoppedFromStack -= UiScreenManager_OnUIScreenPoppedFromStack;
        }

        private void UiScreenManager_OnUIScreenPoppedFromStack(UIScreen uiScreen)
        {
            ///
            if (uiScreen == currentUIScreen)
            {
                onPoppedFromStack.Invoke();
            }

            ///
            UnlaunchCurrentUIScreen();
        }
    }

}