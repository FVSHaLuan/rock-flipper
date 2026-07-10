using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Agame.Marketing
{
    public class SteamStoreStateDetector : ExtendedMonoBehaviour
    {
        private const string ReleaseStateKey = "SteamReleaseState";
        private const string StoreInfoUrlFormat = "https://store.steampowered.com/api/appdetails?filters=price_overview&appids={0}";

        public event System.Action OnStoreStateLoaded;

        public int DiscountPercentage { get; private set; }
        public bool IsGameReleased
        {
            get
            {
                ///
                TryInit();

                ///
                return isGameReleased;
            }
        }

        private bool isGameReleased;
        private bool isLoadingStoreState;
        private string storeInfoUrl = null;

        protected override bool Init()
        {
            LoadReleaseState();

            ///
            return base.Init();
        }

        private void SaveReleaseState(bool released)
        {
            PlayerPrefs.SetString(ReleaseStateKey, released ? GameConst.MainSteamAppId.ToString() : "");
            PlayerPrefs.Save();
        }

        private void LoadReleaseState()
        {
            isGameReleased = PlayerPrefs.GetString(ReleaseStateKey, "") == GameConst.MainSteamAppId.ToString();
        }

        protected override void ExtendedAwake()
        {
            ///
            base.ExtendedAwake();

            ///
            TryLoadingStoreState();
        }

        private void TryLoadingStoreState()
        {
            if (isLoadingStoreState)
            {
                return;
            }

            ///
            StartCoroutine(LoadStoreState());
        }

        private IEnumerator LoadStoreState()
        {
            ///
            TryInit();

            ///
            isLoadingStoreState = true;

            ///
            if (string.IsNullOrWhiteSpace(storeInfoUrl))
            {
                BuildStoreInfoUrl();
            }

            using (var rs = UnityWebRequest.Get(storeInfoUrl))
            {
                ///
                rs.SendWebRequest();

                ///
                while (!rs.isDone)
                {
                    yield return null;
                }

                ///
                if (rs.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogFormat("Failed to load Steam discount: {0}", rs.result);
                    yield break;
                }

                ///
                while (!rs.downloadHandler.isDone)
                {
                    yield return null;
                }

                ///
                if (!string.IsNullOrEmpty(rs.downloadHandler.error))
                {
                    Debug.LogFormat("Failed to load Steam discount: {0}", rs.downloadHandler.error);
                    yield break;
                }

                ///
                var jsonText = rs.downloadHandler.text;
                try
                {
                    DiscountPercentage = SimpleJSON.JSON.Parse(jsonText)[GameConst.MainSteamAppId.ToString()]["data"]["price_overview"]["discount_percent"].AsInt;
                    var initialPrice = SimpleJSON.JSON.Parse(jsonText)[GameConst.MainSteamAppId.ToString()]["data"]["price_overview"]["initial"].AsFloat;

                    ///
                    isGameReleased = initialPrice > 0;
                    SaveReleaseState(isGameReleased);

                    ///
                    Debug.Log($"Loaded Steam Store state, Initial: {initialPrice},  Discount: {DiscountPercentage}%");
                }
                catch (System.Exception e)
                {
                    Debug.Log($"Failed to load Steam store state, couldn't parse the json: {jsonText}, error: {e}");
                    yield break;
                }

                ///
                OnStoreStateLoaded?.Invoke();
            }


            ///
            yield return null;

            ///
            isLoadingStoreState = false;
        }

        private void BuildStoreInfoUrl()
        {
            storeInfoUrl = string.Format(StoreInfoUrlFormat, GameConst.MainSteamAppId);
        }
    }

}