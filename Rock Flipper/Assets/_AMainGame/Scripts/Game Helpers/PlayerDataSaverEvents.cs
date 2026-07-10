using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerDataSaverEvents : ExtendedMonoBehaviour
{
    [SerializeField]
    private UnityEvent onSaved;

    protected void OnEnable()
    {
        entry.playerDataSaver.OnSaved += PlayerDataSaver_OnSaved;
    }

    protected void OnDisable()
    {
        entry.playerDataSaver.OnSaved -= PlayerDataSaver_OnSaved;
    }

    private void PlayerDataSaver_OnSaved()
    {
        onSaved?.Invoke();
    }
}
