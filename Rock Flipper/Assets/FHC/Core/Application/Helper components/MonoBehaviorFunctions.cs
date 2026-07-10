using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class MonoBehaviorFunctions : MonoBehaviour
{
    public void DestroyGameObject()
    {
        Destroy(gameObject);
    }

    public void SetLayer(string layerName)
    {
        gameObject.layer = LayerMask.NameToLayer(layerName);
    }

    public void SetGameObjectDeactiveDelay(float delay)
    {
        StartCoroutine(SetGameObjectDeactiveDelayCoroutine(delay));
    }

    public void UnloadSceneAsync()
    {
        UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(gameObject.scene);
    }

    private IEnumerator SetGameObjectDeactiveDelayCoroutine(float delay)
    {
        ///
        yield return new WaitForSecondsRealtime(delay);

        ///
        gameObject.SetActive(false);
    }
}
