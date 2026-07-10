using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataSaverHelper : MonoBehaviour
{
    public void SaveNow()
    {
        Entry.Instance.playerDataSaver.SaveNow();   
    }

    public void AddUnsavableLock()
    {
        Entry.Instance.playerDataSaver.AddUnsavableLock(this);
    }

    public void RemoveUnsavableLock()
    {
        Entry.Instance.playerDataSaver.RemoveUnsavableLock(this);
    }
}
