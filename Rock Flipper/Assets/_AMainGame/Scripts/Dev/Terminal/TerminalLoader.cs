using CommandTerminal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Terminal))]
public class TerminalLoader : MonoBehaviour
{
    public void Awake()
    {
        if (Entry.Instance.gameSettingObject.Data.enabledTerminal || Application.isEditor)
        {
            GetComponent<Terminal>().enabled = true;
        }
        else
        {
            GetComponent<Terminal>().enabled = false;
        }
    }
}
