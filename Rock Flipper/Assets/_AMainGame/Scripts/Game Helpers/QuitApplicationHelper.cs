using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuitApplicationHelper : MonoBehaviour
{
    private const string BlankSceneName = "Blank";

    public void Quit()
    {
        ///
        Debug.Log("Start quitting...");

        ///
        Entry.Instance.quittingPrompt.gameObject.SetActive(true);

        ///
        if (SceneUtility.GetBuildIndexByScenePath(BlankSceneName) >= 0)
        {
            Debug.LogFormat("Loading scene {0}...", BlankSceneName);
            SceneManager.LoadScene(BlankSceneName);
        }
        else
        {
            Debug.LogFormat("Scene {0} not found!", BlankSceneName);
        }

        if (Application.isEditor)
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
        else
        {
            ///
            Application.Quit();
        }

        ///
        Debug.Log("Quitted!");
    }
}
