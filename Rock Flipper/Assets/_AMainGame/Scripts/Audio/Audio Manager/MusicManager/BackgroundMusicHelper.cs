using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusicHelper : ExtendedMonoBehaviour
{
    [SerializeField]
    private BackgroundMusicPlayer playerPrototype;
    [SerializeField]
    private bool forcesChanged = true;

    public void ChangeMusic(bool isImmediately)
    {
        entry.backgroundMusicManager.ChangeMusic(playerPrototype, isImmediately, forcesChanged, out _);
    }

    public void StopAndDestroy(bool isImmediately)
    {
        entry.backgroundMusicManager.StopAndDestroy(isImmediately);
    }
}
