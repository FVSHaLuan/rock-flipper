using Agame.Balancing;
using Agame.Run;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Agame.Home
{
    public class RunDataSlotView : ExtendedMonoBehaviourHome
    {
        [SerializeField]
        private int slotIndex;

        [Space]
        [SerializeField]
        private float countingEffectDuration = 0.5f;
        [SerializeField]
        private UnifiedText slotNameText;
        [SerializeField]
        private GameObject noDataView;
        [SerializeField]
        private GameObject hasDataView;
        [SerializeField]
        private UnifiedText playTimeText;
        [SerializeField]
        private UnifiedText skillPointsText;
        [SerializeField]
        private UnifiedText bossLevelText;
        [SerializeField]
        private GameObject deleteButton;
        [SerializeField]
        private GameObject notCompatText;

        protected void Start()
        {
            slotNameText.Text = "Slot " + slotIndex;

            ///
            UpdateView(true);
        }

        protected void OnDisable()
        {
            UpdateView(false);
        }

        public void HandleClick()
        {
            ///
            entry.runDataManager.ActiveRunDataIndex = slotIndex;

            ///
            var runData = entry.runDataManager.GetRunDataObject(slotIndex).Data;

            ///
            if (runData.InitedRun && runData.CompatVersion != entry.compatManager.CurrentCompatVersion)
            {
                entry.GeneralDialog.Show("This slot was saved with an older version of the game that is no longer supported.\r\nSorry for the inconvenience :(", false, null);
                return;
            }

            ///
            entry.visualSceneLoader.Load(GameScene.Run);
        }

        public void DeleteData()
        {
            ///
            entry.runDataManager.ActiveRunDataIndex = slotIndex;

            ///
            var runDataObject = entry.runDataManager.GetRunDataObject(slotIndex);
            var runDataObjectSlot0 = entry.runDataManager.GetRunDataObject(0);
            runDataObject.CopyDefaultDataToCurrentData(runDataObjectSlot0);
            entry.playerDataSaver.SaveNow();

            ///
            UpdateView(false);
        }

        private void UpdateView(bool playCountingEffect)
        {
            var runData = entry.runDataManager.GetRunDataObject(slotIndex).Data;

            ///
            if (runData.InitedRun)
            {
                ViewData(runData, playCountingEffect);
            }
            else
            {
                ViewNoData();
            }
        }

        private void ViewNoData()
        {
            noDataView.SetActive(true);
            hasDataView.SetActive(false);
            deleteButton.SetActive(false);
        }

        private void ViewData(RunData runData, bool playCountingEffect)
        {
            ///
            noDataView.SetActive(false);
            hasDataView.SetActive(true);
            deleteButton.SetActive(true);

            ///
            if (playCountingEffect)
            {
                StartCoroutine(PlayCountingEffect(runData));
            }
            else
            {
                ViewDataCounting(runData, 1f);
            }

            ///
            notCompatText.SetActive(runData.CompatVersion != entry.compatManager.CurrentCompatVersion);
        }

        private IEnumerator PlayCountingEffect(RunData runData)
        {
            float elapsedTime = 0f;

            while (elapsedTime < countingEffectDuration)
            {
                elapsedTime += Time.deltaTime;
                float progress = Mathf.Clamp01(elapsedTime / countingEffectDuration);

                ///
                ViewDataCounting(runData, progress);

                ///
                yield return null;
            }

            // Ensure final values are set
            ViewDataCounting(runData, 1);
        }

        private void ViewDataCounting(RunData runData, float progress)
        {
            throw new System.NotImplementedException();
            ///
            //var playTime = Mathf.Lerp(0, runData.PlayTime, progress);
            //playTimeText.Text = TimeStringHelper.GetStringFromSeconds(playTime);

            /////
            //var skillPoints = (int)Mathf.Lerp(0, runData.lastCalculatedTotalSkillPoints, progress);
            //skillPointsText.Text = skillPoints.ToLargeNumberString();

            /////
            //var bossLevel = (int)Mathf.Lerp(0, runData.BossCoreLevel, progress);
            //if (bossLevel < GameBalance.FinalBossLevel)
            //{
            //    bossLevelText.Text = bossLevel.ToStringCached();
            //}
            //else if (bossLevel == GameBalance.FinalBossLevel)
            //{
            //    bossLevelText.Text = "Final";
            //}
            //else
            //{
            //    bossLevelText.Text = "Beat All!";
            //}

        }
    }

}
