#if !DISABLESTEAMWORKS
using Steamworks; 
#endif
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Agame.Steamworks
{
    public abstract class SubmitLeaderboardScoreToSteam : ExtendedMonoBehaviour
    {
#if !DISABLESTEAMWORKS
        private float SubmitTimeInterval = 60;

        [SerializeField]
        private string leaderboardId;
        [SerializeField]
        private ELeaderboardUploadScoreMethod scoreMethod = ELeaderboardUploadScoreMethod.k_ELeaderboardUploadScoreMethodKeepBest;

        private CallResult<LeaderboardFindResult_t> findLeaderboardCallResult;
        private CallResult<LeaderboardScoreUploaded_t> uploadScoreCallResult;
        private SteamLeaderboard_t steamLeaderboard;
        private bool foundSteamLeaderboard = false;

        private int score;
        private int[] scoreDetails = new int[10];
        private int scoreDetailCount = 0;
        private int scoreId = -1;
        private int submittingScoreId = -1;
        private int lastSuccessfullySubmitedScoreId = -1;
        private float lastTimeSubmittedScore = float.MinValue;

        protected override void ExtendedAwake()
        {
            ///
            base.ExtendedAwake();

            ///
            SteamManager.Instance.DoAfterInited(InitLeaderboard);
        }

        private void InitLeaderboard()
        {
            ///
            findLeaderboardCallResult = CallResult<LeaderboardFindResult_t>.Create(OnFindLeaderboardCallResult);
            uploadScoreCallResult = CallResult<LeaderboardScoreUploaded_t>.Create(OnUploadScoreCallResult);

            ///
            var callHandle = SteamUserStats.FindLeaderboard(leaderboardId);
            findLeaderboardCallResult.Set(callHandle);
        }

        private void OnFindLeaderboardCallResult(LeaderboardFindResult_t result, bool bIOFailure)
        {
            ///
            if (bIOFailure)
            {
                ///
                Debug.LogErrorFormat("Failed to get leaderboard {0}.", leaderboardId);

                ///
                return;
            }

            ///
            if (result.m_bLeaderboardFound != 1)
            {
                ///
                Debug.LogErrorFormat("Leaderboard {0} not found.", leaderboardId);

                ///
                return;
            }

            ///
            steamLeaderboard = result.m_hSteamLeaderboard;
            foundSteamLeaderboard = true;
        }

        protected void SetScore(int score, List<int> scoreDetails)
        {
#if !BSB_VER_DEMO
            ///
            if (gameSetting.enabledTerminal)
            {
                return;
            }

            ///
            scoreId++;

            ///
            this.score = score;
            if (scoreDetails != null)
            {
                scoreDetailCount = scoreDetails.Count;
                for (int i = 0; i < scoreDetails.Count; i++)
                {
                    this.scoreDetails[i] = scoreDetails[i];
                }
            }
            else
            {
                scoreDetailCount = 0;
            }
#endif
        }

        protected void LateUpdate()
        {
            ///
            if (scoreId < 0)
            {
                return;
            }

            ///
            if ((Time.realtimeSinceStartup - lastTimeSubmittedScore) < SubmitTimeInterval)
            {
                return;
            }

            ///
            if (submittingScoreId >= 0)
            {
                return;
            }

            ///
            if (scoreId == lastSuccessfullySubmitedScoreId)
            {
                return;
            }

            ///
            if (!foundSteamLeaderboard)
            {
                return;
            }

            ///
            SubmitToSteam();
        }

        [ContextMenu("SubmitToSteam")]
        private void SubmitToSteam()
        {
            ///
            submittingScoreId = scoreId;
            lastTimeSubmittedScore = Time.realtimeSinceStartup;

            ///
            var callHandle = SteamUserStats.UploadLeaderboardScore(steamLeaderboard, scoreMethod, score, scoreDetails, scoreDetailCount);
            uploadScoreCallResult.Set(callHandle);
        }

        private void OnUploadScoreCallResult(LeaderboardScoreUploaded_t result, bool bIOFailure)
        {
            ///
            var savedSubmittingScoreId = submittingScoreId;
            submittingScoreId = -1;

            ///
            if (bIOFailure)
            {
                return;
            }

            ///
            if (result.m_bSuccess != 1)
            {
                return;
            }

            ///
            lastSuccessfullySubmitedScoreId = savedSubmittingScoreId;
        } 
#endif
    }

}