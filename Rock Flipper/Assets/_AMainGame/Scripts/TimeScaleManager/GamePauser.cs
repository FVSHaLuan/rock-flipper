using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePauser : ExtendedMonoBehaviour
{
    public void Pause()
    {
        entry.timeScaleManager.AddPauseGame(this);
    }

    public void Unpause()
    {
        entry.timeScaleManager.RemovePauseGame(this);
    }
}
