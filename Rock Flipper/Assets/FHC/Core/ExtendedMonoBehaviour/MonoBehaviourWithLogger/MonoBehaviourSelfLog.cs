using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct MonoBehaviourSelfLog
{
    public bool enableLogger;
    public bool alsoLogToUnityConsole;
    [TextArea(10, 10)]
    public string selfLogs;
}
