using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettingHelper : ExtendedMonoBehaviour
{
    public void SaveSetting()
    {
        entry.gameSettingObject.SaveData();
    }
}
