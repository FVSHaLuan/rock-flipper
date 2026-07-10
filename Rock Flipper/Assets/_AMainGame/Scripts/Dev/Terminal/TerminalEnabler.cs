using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerminalEnabler : ExtendedMonoBehaviour
{
    public void EnableTerminal()
    {
        entry.GameSetting.enabledTerminal = true;
        entry.gameSettingObject.SaveData();
    }
}
