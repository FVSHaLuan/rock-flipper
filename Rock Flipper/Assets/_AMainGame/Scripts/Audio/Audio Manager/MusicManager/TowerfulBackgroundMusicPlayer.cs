using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Agame
{
    public class TowerfulBackgroundMusicPlayer : BackgroundMusicPlayer
    {
        [SerializeField]
        private List<AudioClip> originalClips;
        [SerializeField]
        private AudioClip demoClip;

        [Space]
        [SerializeField]
        private bool replayOnDemoMusicUsageChange;

        protected override void DetermineAudioClip()
        {
            ///
            base.DetermineAudioClip();

            ///
            int count = originalClips.Count + ((gameSetting.UseDemoMusic && demoClip != null) ? 1 : 0);
            int index = Random.Range(0, count);

            ///
            if (index < originalClips.Count)
            {
                AudioSource.clip = originalClips[index];
            }
            else
            {
                AudioSource.clip = demoClip;
            }
        }

        protected override void OnDisable()
        {
            ///
            base.OnDisable();

            ///
            gameSetting.OnDemoMusicUsageChanged -= GameSetting_OnDemoMusicUsageChanged;
        }

        protected override void OnEnable()
        {
            ///
            base.OnEnable();

            ///
            gameSetting.OnDemoMusicUsageChanged += GameSetting_OnDemoMusicUsageChanged;
        }

        private void GameSetting_OnDemoMusicUsageChanged()
        {
            if (replayOnDemoMusicUsageChange)
            {
                DetermineAudioClip();
                AudioSource.Play();
            }
        }
    }

}