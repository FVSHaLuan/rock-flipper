using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

[RequireComponent(typeof(VideoPlayer))]
public class GameAudioControllerVideoPlayer : GameAudioControllerBase
{
    private VideoPlayer videoPlayer;

    private VideoPlayer VideoPlayer
    {
        get
        {
            ///
            if (videoPlayer == null)
            {
                videoPlayer = GetComponent<VideoPlayer>();
            }

            ///
            return videoPlayer;
        }
    }

    protected override float FinalVolume
    {
        get
        {
            if (VideoPlayer.controlledAudioTrackCount > 0)
            {
                return VideoPlayer.GetDirectAudioVolume(0);
            }
            else
            {
                return 0;
            }
        }

        set
        {
            for (ushort i = 0; i < VideoPlayer.controlledAudioTrackCount; i++)
            {
                VideoPlayer.SetDirectAudioVolume(i, value);
            }
        }
    }
    protected override float FinalPitch { get => 1; set { } }

    protected override float Length => (float)VideoPlayer.length;

    protected override float CurrentTime { get => (float)VideoPlayer.time; set => VideoPlayer.time = value; }

    protected override bool IsLooping => VideoPlayer.isLooping;

    protected override bool IsPlaying => VideoPlayer.isPlaying;

    protected override void PlayImmediately()
    {
        VideoPlayer.Play();
    }

    protected override void _StopImmediately()
    {
        VideoPlayer.Stop();
    }

#if UNITY_EDITOR
    public override void Reset()
    {
        ///
        base.Reset();

        ///
        GetComponent<VideoPlayer>().audioOutputMode = VideoAudioOutputMode.Direct;
    }
#endif
}
