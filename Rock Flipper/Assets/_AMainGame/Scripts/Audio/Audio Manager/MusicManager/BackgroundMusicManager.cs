using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusicManager : ExtendedMonoBehaviour
{
    [SerializeField]
    private float crossfadeTime = 0.5f;

    private BackgroundMusicPlayer currentMusicPlayer;

    public BackgroundMusicPlayer CurrentMusicPlayer => currentMusicPlayer;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="musicPlayerPrototype"></param>
    /// <param name="isImmediately"></param>
    /// <param name="forced">process change even if the prototype is the same as the prototype of currentMusicPlayer</param>
    /// <returns></returns>
    public BackgroundMusicPlayer ChangeMusic(BackgroundMusicPlayer musicPlayerPrototype, bool isImmediately, bool forced, out bool changed)
    {
        ///
        if (!forced && currentMusicPlayer != null && currentMusicPlayer.PrototypeId == musicPlayerPrototype.GetHashCode())
        {
            ///
            changed = false;

            ///
            return currentMusicPlayer;
        }

        ///
        var fadingTime = isImmediately ? -1 : crossfadeTime;

        ///
        if (currentMusicPlayer != null)
        {
            currentMusicPlayer.StopPlayingAndDestroy(fadingTime);
        }

        ///
        currentMusicPlayer = Instantiate(musicPlayerPrototype);

        ///
        currentMusicPlayer.transform.SetParent(transform, false);

        ///
        currentMusicPlayer.StartPlaying(fadingTime, musicPlayerPrototype.GetHashCode());

        ///
        changed = true;

        ///
        return currentMusicPlayer;
    }

    public void StopAndDestroy(bool isImmediately)
    {
        ///
        if (currentMusicPlayer != null)
        {
            ///
            var fadingTime = isImmediately ? -1 : crossfadeTime;

            ///
            currentMusicPlayer.StopPlayingAndDestroy(fadingTime);
        }
    }
}
