#if !DISABLESTEAMWORKS
using Steamworks;
#endif
using System.Collections;
using System.Collections.Generic;
using BT.FeatureBranching;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace BT.UI
{
    public class FeedbackPopup : ExtendedMonoBehaviour
    {
        private const string GoogleFormUrl = @"https://docs.google.com/forms/d/e/1FAIpQLSdm1BmI7aIWNf3mLJhoWgiyAeyPr8Mhb6g5QpPJVoIy4FKBug/formResponse";

        [SerializeField]
        private TMP_InputField emailInput;
        [SerializeField]
        private TMP_InputField feedbackInput;
        [SerializeField]
        private Selectable sendButton;
        [SerializeField]
        private int minFeedbackCharacterCount = 10;

        [Space]
        [SerializeField]
        private UnifiedText statusText;

        [Space]
        [SerializeField]
        private UnityEvent onFinishSubmitting;

        private Dictionary<string, string> formData;

        [ContextMenu("Show")]
        public void Show()
        {
            gameObject.SetActive(true);
            statusText.gameObject.SetActive(false);
        }

        public void Submit()
        {
            StartCoroutine(SubmitCoroutine());
        }

        protected void Update()
        {
            sendButton.interactable = feedbackInput.text.Length >= minFeedbackCharacterCount;
        }

        private IEnumerator SubmitCoroutine()
        {
            ///
            if (formData == null)
            {
                formData = new Dictionary<string, string>();
            }

            //

            ///
            formData["entry.2131754366"] = emailInput.text;
            formData["entry.184128290"] = feedbackInput.text;
            formData["entry.2115987557"] = GetUserId();
            formData["entry.115957134"] = Application.platform.ToString();
            formData["entry.74311494"] = StoreInfo.CurrentStore.ToString();
            formData["entry.832556693"] = Application.version;
            // formData["entry.1316411100"] = playerData.FinishedRunCount.ToString();

            ///
            statusText.gameObject.SetActive(true);
            statusText.Text = "Sending...";

            ///
            using (var rs = UnityWebRequest.Post(GoogleFormUrl, formData))
            {
                ///
                rs.SendWebRequest();

                ///
                while (!rs.isDone)
                {
                    yield return null;
                }

                ///
                statusText.Text = string.Format("Done: {0}. {1}", rs.result, rs.result == UnityWebRequest.Result.Success ? "Thank you!" : "");

                ///
                if (rs.result == UnityWebRequest.Result.Success)
                {
                    ClearFeedback();
                }
            }

            ///
            yield return null;

            ///
            onFinishSubmitting?.Invoke();
        }

        private string GetUserId()
        {
            ///
            if (SteamManager.Initialized)
            {
#if !DISABLESTEAMWORKS
                return string.Format("steam:{0}", SteamUser.GetSteamID().m_SteamID);
#endif
            }

            ///
            return string.Format("pseudo:{0}", playerData.PseudoUserId);
        }

        private void ClearFeedback()
        {
            feedbackInput.text = string.Empty;
        }
    }

}