using Agame.Localization;
using Agame.ShortHands;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;

namespace Agame.UI
{
    public class GeneralDialogLauncher : ExtendedMonoBehaviour
    {
        [SerializeField, TextArea]
        private string message;
        [SerializeField, Tooltip("Used instead of message when set")]
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
            var rawMessage = localizedMessage.IsValidAndNotEmpty() ? localizedMessage.GetLocalizedString() : message;
            var finalMessage = string.IsNullOrWhiteSpace(rawMessage) ? string.Empty : rawMessage;
            finalMessage = entry.shortHandManager.Apply(finalMessage);

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