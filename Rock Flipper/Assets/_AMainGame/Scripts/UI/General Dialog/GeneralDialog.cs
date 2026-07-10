using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.Runtime.CompilerServices.RuntimeHelpers;

namespace Agame.UI
{
    public class GeneralDialog : ExtendedMonoBehaviour
    {
        public delegate void GeneralDialogCallback(GeneralDialogResult result);

        [SerializeField]
        private UnifiedText messageText;
        [SerializeField]
        private GameObject cancelButton;

        public DialogOption option;

        [System.Serializable]
        public struct DialogOption
        {
            public string message;
            public bool cancellable;
            public bool callbackImmediatelyOnCancel;
            public bool callbackImmediatelyOnOK;
            public GeneralDialogCallback callback;
            public bool doNotCloseOnCancel;
            public bool doNotCloseOnOK;
        }

        protected override void ExtendedAwake()
        {
            ///
            base.ExtendedAwake();
        }

        public void Show(string message, bool cancellable, GeneralDialogCallback callback)
        {
            var option = new DialogOption()
            {
                callback = callback,
                message = message,
                cancellable = cancellable,
            };

            ///
            Show(option);
        }

        public void Show(DialogOption option)
        {
            ///
            this.option = option;

            ///
            cancelButton.SetActive(option.cancellable);

            ///
            if (messageText != null)
            {
                messageText.Text = option.message;
            }

            ///
            gameObject.SetActive(true);
        }

        public void Cancel()
        {
            ///
            if (!option.cancellable)
            {
                return;
            }

            ///
            if (option.callbackImmediatelyOnCancel)
            {
                option.callback?.Invoke(GeneralDialogResult.Cancelled);
                TryClose(false);
            }
            else
            {
                TryClose(false);
                option.callback?.Invoke(GeneralDialogResult.Cancelled);
            }
        }

        public void OK()
        {
            ///
            if (option.callbackImmediatelyOnOK)
            {
                option.callback?.Invoke(GeneralDialogResult.OK);
                TryClose(true);
            }
            else
            {
                TryClose(true);
                option.callback?.Invoke(GeneralDialogResult.OK);
            }
        }

        private void TryClose(bool isOK)
        {
            if (isOK && option.doNotCloseOnOK)
            {
                return;
            }
            if (!isOK && option.doNotCloseOnCancel)
            {
                return;
            }

            ///
            gameObject.SetActive(false);
        }
    }
}