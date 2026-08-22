using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MonoBehaviourExtensions
{
    public static void ToggleActiveState(this MonoBehaviour monoBehaviour)
    {
        monoBehaviour.enabled = !monoBehaviour.enabled;
    }

    public static void ToggleActiveState(this GameObject go)
    {
        go.SetActive(!go.activeSelf);
    }
}
