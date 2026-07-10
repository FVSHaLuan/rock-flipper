using I2.Loc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace BT.UI
{
    public class GeneralDialogLauncher : ExtendedMonoBehaviour
    {
        [SerializeField, TextArea]
        private string message;
        [SerializeField]
        private LocalizedString localizedMessage;
        [SerializeField]
        private bool isCancellable;
        [SerializeField]
        private bool callbackImmediatelyOnCancel;
        [SerializeField]
        private bool callbackImmediatelyOnOK;
        [SerializeField]
        private bool doNotCloseOnCancel;
        [SerializeField]
        private bool doNotCloseOnOK;

        [Space]
        [SerializeField]
        private UnityEvent onOK;
        [SerializeField]
        private UnityEvent onCancelled;

        [ContextMenu("Launch")]
        public void Launch()
        {
            ///
            var finalMessage = string.IsNullOrWhiteSpace(localizedMessage) ? message : localizedMessage.ToString();

            ///
            var option = new GeneralDialog.DialogOption()
            {
                callback = FinishedCallback,
                message = finalMessage,
                cancellable = isCancellable,
                callbackImmediatelyOnCancel = callbackImmediatelyOnCancel,
                callbackImmediatelyOnOK = callbackImmediatelyOnOK,
                doNotCloseOnCancel = doNotCloseOnCancel,
                doNotCloseOnOK = doNotCloseOnOK,
            };

            ///
            entry.GeneralDialog.Show(option);
        }

        private void FinishedCallback(GeneralDialogResult result)
        {
            if (!isCancellable || result == GeneralDialogResult.OK)
            {
                onOK.Invoke();
            }
            else
            {
                onCancelled.Invoke();
            }
        }
    }

}