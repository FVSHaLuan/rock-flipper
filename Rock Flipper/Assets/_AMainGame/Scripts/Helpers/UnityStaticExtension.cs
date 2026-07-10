using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UnityStaticExtension
{
    public static void InvokeDelay(this MonoBehaviour monoBehaviour, System.Action action, float delay, bool scaledTime = true)
    {
        monoBehaviour?.StartCoroutine(InvokeDelay(action, delay, scaledTime));
    }

    private static IEnumerator InvokeDelay(System.Action action, float delay, bool scaledTime)
    {
        ///
        if (scaledTime)
        {
            yield return new WaitForSeconds(delay);
        }
        else
        {
            yield return new WaitForSecondsRealtime(delay);
        }

        ///
        action?.Invoke();
    }
}
