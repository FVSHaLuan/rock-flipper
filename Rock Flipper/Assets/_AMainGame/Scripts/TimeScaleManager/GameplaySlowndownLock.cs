using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplaySlowndownLock : ExtendedMonoBehaviour
{
    protected void OnEnable()
    {
        entry.timeScaleManager.AddGameplaySlowdown(this);
    }

    protected void OnDisable()
    {
        entry.timeScaleManager.RemoveGameplaySlowdown(this);
    }
}
