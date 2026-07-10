using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoBehaviourWithLogger : MonoBehaviour
{
    [SerializeField]
    private MonoBehaviourSelfLog selfLog;

    protected bool EnableLogger => selfLog.enableLogger;

    /// <summary>
    /// Log to SelfLogs
    /// </summary>
    /// <param name="str"></param>
    public void Log(string str)
    {
        if (EnableLogger)
        {
            var logString = GetLogString(str);

            ///
            if (selfLog.alsoLogToUnityConsole)
            {
                Debug.Log(logString);
            }

            ///
            AddToSelfLog(logString);
        }
    }

    protected void LogFormat(string format, params object[] args)
    {
        if (EnableLogger)
        {
            ///
            var logString = GetLogString(format, args);

            ///
            if (selfLog.alsoLogToUnityConsole)
            {
                Debug.Log(logString);
            }

            ///
            AddToSelfLog(logString);
        }
    }

    private void AddToSelfLog(string logString)
    {
        if (selfLog.selfLogs == null)
        {
            selfLog.selfLogs = logString;
        }
        else
        {
            selfLog.selfLogs += "\n" + logString;
        }
    }

    private string GetLogString(string str)
    {
        return string.Format("[{0}] {1}", Time.frameCount, str);
    }

    private string GetLogString(string format, params object[] args)
    {
        return string.Format("[{0}] {1}", Time.frameCount, string.Format(format, args));
    }

#if UNITY_EDITOR
    [ContextMenu("Editor_ClearSelfLog")]
    private void Editor_ClearSelfLog()
    {
        selfLog.selfLogs = null;
    }
#endif
}
