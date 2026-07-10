using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Obsolete("This is old controller to control music when do Reset")]
public class BackgroundMusicController : ExtendedMonoBehaviour
{
    //[SerializeField]
    //private BackgroundMusicPlayer introMusicPrototype;
    //[SerializeField]
    //private BackgroundMusicPlayer loopMusicPrototype;
    //[SerializeField]
    //private BackgroundMusicPlayer outroMusicPrototype;

    //[Space]
    //[SerializeField]
    //private bool startFading;
    //[SerializeField]
    //private bool outroFading;
    //[SerializeField]
    //private bool introLoopFading;
    //[SerializeField]
    //private bool outroIntroFading;

    //protected override void ExtendedAwake()
    //{
    //    ///
    //    base.ExtendedAwake();

    //    ///
    //    PlayFromIntro(!startFading);

    //    ///
    //    entry.prestiger.OnBeforeReset += Prestiger_OnBeforeReset;
    //}

    //private void Prestiger_OnBeforeReset()
    //{
    //    ///
    //    PlayFromOutro(!outroFading);
    //}

    //private void PlayFromOutro(bool isImmediately)
    //{
    //    ///
    //    var outroMusic = entry.backgroundMusicManager.ChangeMusic(outroMusicPrototype, isImmediately);

    //    ///
    //    outroMusic.OnMusicStopped += OutroMusicPrototype_OnMusicStopped;
    //}

    //private void OutroMusicPrototype_OnMusicStopped(BackgroundMusicPlayer outroMusic)
    //{
    //    ///
    //    if (outroMusic.IsStopping)
    //    {
    //        return;
    //    }

    //    ///
    //    PlayFromIntro(!outroIntroFading);
    //}

    //private void PlayFromIntro(bool isImmediately)
    //{
    //    ///
    //    var introMusic = entry.backgroundMusicManager.ChangeMusic(introMusicPrototype, isImmediately);

    //    ///
    //    introMusic.OnMusicStopped += IntroMusic_OnMusicStopped;
    //}

    //private void IntroMusic_OnMusicStopped(BackgroundMusicPlayer introMusic)
    //{
    //    ///
    //    if (introMusic.IsStopping)
    //    {
    //        return;
    //    }

    //    ///
    //    entry.backgroundMusicManager.ChangeMusic(loopMusicPrototype, !introLoopFading);
    //}
}
