using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioGlitcher : ExtendedMonoBehaviourWithTime
{
    [SerializeField]
    private float interval = 0.15f;

    private AudioSource audioSource;

    [System.NonSerialized]
    private AudioGlitcher copiedInstance;

    [ContextMenu("StartGlitching")]
    public void StartGlitching()
    {
        ///
        if (copiedInstance != null)
        {
            ///
            copiedInstance.StartGlitching();

            ///
            return;
        }

        ///
        audioSource = GetComponent<AudioSource>();

        ///
        if (audioSource == null)
        {
            return;
        }

        ///
        StopAllCoroutines();

        ///
        StartCoroutine(Glitch());
    }

    [ContextMenu("StopGlitching")]
    public void StopGlitching()
    {
        ///
        if (copiedInstance != null)
        {
            ///
            copiedInstance.StopGlitching();

            ///
            return;
        }

        ///
        StopAllCoroutines();
    }

    [ContextMenu("CopyToCurrentBackgroundMusic")]
    public void CopyToCurrentBackgroundMusic()
    {
        ///
        copiedInstance = this.CopyToGameObject(entry.backgroundMusicManager.CurrentMusicPlayer.gameObject);
    }

    [ContextMenu("CopyToCurrentBackgroundMusicAndStartGlitching")]
    public void CopyToCurrentBackgroundMusicAndStartGlitching()
    {
        ///
        copiedInstance = this.CopyToGameObject(entry.backgroundMusicManager.CurrentMusicPlayer.gameObject);

        ///
        StartGlitching();
    }

    private IEnumerator Glitch()
    {
        while (true)
        {
            ///
            var savedTime = audioSource.time;

            ///
            yield return new WaitForUniversalTime(interval, timeScaleMode);

            ///
            if (audioSource.clip == null)
            {
                yield break;
            }

            ///
            audioSource.time = Mathf.Clamp(savedTime, 0, audioSource.clip.length);
        }
    }
}
