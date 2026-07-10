using FH.Core.Architecture.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class PooledGameAudioController : GameAudioController
{
    public bool IsInPool { get; set; } = false;

    public int PlayId { get; private set; }

    public void Play(PooledAudioPlayer pooledAudioPlayer, int playId)
    {
        ///
        PlayId = playId;
        AudioSource.clip = pooledAudioPlayer.AudioClip;
        AudioSource.priority = pooledAudioPlayer.Priority;
        MixerGroup = pooledAudioPlayer.mixerGroup;
        selfVolume = pooledAudioPlayer.selfVolume;
        gameAudioChannel = pooledAudioPlayer.gameAudioChannel;
        staticVolumeGroup = pooledAudioPlayer.staticVolumeGroup;
        EndTrimmingTime = pooledAudioPlayer.EndTrimmingTime;
        StartTrimmingTime = pooledAudioPlayer.StartTrimmingTime;

        ///
        UpdateStaticParameters();
        UpdateAudioSourceVolume();

        ///
        AudioSource.time = 0;

        ///
        gameObject.SetActive(true);

        ///
        AudioSource.Play();
    }

    public override void Update()
    {
        ///
        base.Update();

        ///
        if (!IsInPool && !AudioSource.isPlaying)
        {
            BackToPool();
            return;
        }
    }

    public void BackToPool()
    {
        ///
        entry.pooledAudioManager.PutBackToPool(this);
        gameObject.SetActive(false);

        ///
        transform.SetParent(entry.pooledObjectsRoot);
    }
}
