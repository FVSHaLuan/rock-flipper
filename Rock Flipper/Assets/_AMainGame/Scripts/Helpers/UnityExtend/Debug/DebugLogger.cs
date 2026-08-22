using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugLogger : MonoBehaviour
{
    public void LogString(string str)
    {
        Debug.Log(str);
    }

    public void LogWarning(string str)
    {
        Debug.LogWarning(str);
    }

    public void LogError(string str)
    {
        Debug.LogError(str);
    }
}
