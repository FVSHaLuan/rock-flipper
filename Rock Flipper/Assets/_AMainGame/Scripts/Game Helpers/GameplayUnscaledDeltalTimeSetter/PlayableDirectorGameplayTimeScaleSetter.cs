using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[RequireComponent(typeof(PlayableDirector))]
public class PlayableDirectorGameplayTimeScaleSetter : GameplayTimeScaleSetter
{
    private PlayableDirector playableDirector;

    protected override void ExtendedAwake()
    {
        ///
        base.ExtendedAwake();

        ///
        playableDirector = GetComponent<PlayableDirector>();
    }

    protected override void Set(bool useUnscaledTime)
    {
        if (useUnscaledTime)
        {
            playableDirector.timeUpdateMode = DirectorUpdateMode.UnscaledGameTime;
        }
        else
        {
            playableDirector.timeUpdateMode = DirectorUpdateMode.GameTime;
        }
    }
}
