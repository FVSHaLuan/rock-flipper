using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BackgroundMusicPlayer : GameAudioController
{
    public event System.Action<BackgroundMusicPlayer> OnMusicStopped;

    public static int MusicId { get; private set; }
    public static string MusicName { get; private set; }

    [Header("BackgroundMusicPlayer")]
    [SerializeField]
    private UnityEvent onStopped;

    private bool startedPlaying = false;
    private bool invokedMusicStoppedEvent = false;

    public bool IsStopping { get; private set; } = false;
    public int PrototypeId { get; private set; }

    protected virtual void DetermineAudioClip() { }

    public void StartPlaying(float fadingTime, int prototypeId)
    {
        DetermineAudioClip();

        ///
        PrototypeId = prototypeId;

        ///
        startedPlaying = true;

        ///
        gameObject.SetActive(true);
        StartCoroutine(Fade(0, 1, fadingTime, null));
        AudioSource.Play();

        ///
        MusicId = AudioSource.clip == null ? 0 : AudioSource.clip.GetHashCode();
        MusicName = AudioSource.clip == null ? string.Empty : AudioSource.clip.name;
    }

    public void StopPlayingAndDestroy(float fadingTime)
    {
        ///
        IsStopping = true;

        ///
        var effectiveFadingTime = fadingTime * ControlVolume;
        StartCoroutine(Fade(1, 0, effectiveFadingTime, Finish));
    }

    private IEnumerator Fade(float startVolume, float endVolume, float time, System.Action callback)
    {
        ///
        if (time <= 0)
        {
            ControlVolume = endVolume;
            callback?.Invoke();
            yield break;
        }

        ///
        float t = 0;
        while (t <= time)
        {
            ///
            t += Time.unscaledDeltaTime;

            ///
            var volume = Mathf.Lerp(startVolume, endVolume, Mathf.Clamp01(t / time));
            ControlVolume = volume;

            ///
            yield return null;
        }
        ;

        ///
        callback?.Invoke();
    }

    private void Finish()
    {
        Destroy(gameObject);
    }

    public override void Update()
    {
        ///
        base.Update();

        ///
        if (!invokedMusicStoppedEvent && startedPlaying && !AudioSource.isPlaying)
        {
            invokedMusicStoppedEvent = true;
            OnMusicStopped?.Invoke(this);
            onStopped?.Invoke();
        }
    }

#if UNITY_EDITOR
    public override void Reset()
    {
        ///
        base.Reset();

        ///
        gameAudioChannel = GameAudioChannel.Music;
    }
#endif
}
