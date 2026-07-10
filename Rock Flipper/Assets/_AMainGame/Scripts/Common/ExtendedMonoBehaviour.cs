using System.Collections;
using System.Collections.Generic;
using BT;
using BT.Balancing;
using UnityEngine;

public abstract class ExtendedMonoBehaviour : MonoBehaviourWithInit
{
    private bool foundReferences = false;

    protected Entry entry { get; private set; }
    protected CommonEntry CommonEntry => CommonEntry.CommonInstance;
    protected PlayerData playerData { get; private set; }
    protected GeneralPool generalPool { get; private set; }
    protected GameSetting gameSetting { get; private set; }

    protected GameBalance gameBalance => entry.gameBalance;
    protected float GameplayUnscaledDeltaTime => entry.timeScaleManager.GameplayUnscaledDeltaTime;
    /// <summary>
    /// Ignore gameplay slow effect
    /// </summary>
    protected float GameplayUnscaledDeltaTimeAbsolute => entry.timeScaleManager.GameplayUnscaledDeltaTimeAbsolute;
    protected float GameplayUnscaledTime => entry.timeScaleManager.GameplayUnscaledTime;
    protected float GameplayUnscaledTimeAbsolute => entry.timeScaleManager.GetTime(TimeScaleMode.GameplayUnscaledTimeAbsolute);

    protected bool IsLoadingScreenNullOrFinished => BT.UI.LoadingScreen.LoadingScreenHandle.IsNullOrFinished;

    protected GeneralPool CurrentSceneGeneralPool => CommonEntry.GeneralPool;

    public ExtendedMonoBehaviour()
    {
        ///
        TryGetReferences();

        ///
        if (!foundReferences)
        {
            Entry.OnHadInstance += Entry_OnHadInstance;
        }
    }

    ~ExtendedMonoBehaviour()
    {
        Entry.OnHadInstance -= Entry_OnHadInstance;
    }

    private void Entry_OnHadInstance()
    {
        ///
        TryGetReferences();
    }

    public sealed override bool TryInit()
    {
        ///
        if (Inited)
        {
            return true;
        }

        ///
        if (!TryGetReferences())
        {
            return false;
        }

        ///
        return base.TryInit();
    }

    private bool TryGetReferences()
    {
        ///
        if (foundReferences)
        {
            return true;
        }

        ///
        entry = Entry.BareInstance;

        ///
        if (entry != null)
        {
            ///
            playerData = entry.PlayerDataObject.Data;
            generalPool = entry.GeneralPool;
            gameSetting = entry.gameSettingObject.Data;

            ///
            foundReferences = true;

            ///
            return true;
        }
        else
        {
            return false;
        }
    }

    protected float GetDeltaTime(TimeScaleMode timeScaleMode)
    {
        return entry.timeScaleManager.GetDeltaTime(timeScaleMode);
    }

    protected float GetTimeScale(TimeScaleMode timeScaleMode)
    {
        return entry.timeScaleManager.GetTimeScale(timeScaleMode);
    }

    protected float GetTime(TimeScaleMode timeScaleMode)
    {
        return entry.timeScaleManager.GetTime(timeScaleMode);
    }

    public static string GetGameObjectHierarchy(GameObject targetGameObject, GameObject topGameObject, bool includeTopGameObject)
    {
        ///
        var obj = targetGameObject;

        ///
        string path = "/" + obj.name;

        ///
        while (obj.transform.parent != topGameObject || includeTopGameObject)
        {
            ///
            obj = obj?.transform?.parent?.gameObject;
            path = "/" + ((obj != null) ? obj.name : "<NULL>") + path;

            ///
            if (obj == topGameObject || obj == null)
            {
                break;
            }
        }

        ///
        return path;
    }

    public string GetGameObjectHierarchy(GameObject topGameObject, bool includeTopGameObject)
    {
        return GetGameObjectHierarchy(gameObject, topGameObject, includeTopGameObject);
    }

#if UNITY_EDITOR
    [ContextMenu("Editor_LogHierarchyPath")]
    private void Editor_LogHierarchyPath()
    {
        Debug.Log(GetGameObjectHierarchy(null, true));
    }
#endif
}

