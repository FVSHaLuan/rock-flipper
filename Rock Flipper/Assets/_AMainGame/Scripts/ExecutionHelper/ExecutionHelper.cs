using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Camera))]
public class ExecutionHelper : MonoBehaviour
{
    private event Action OnUpdate;
    private event Action OnLateUpdate;
    private event Action OnEndOfFrame;
    private event Action OnPreCullEvent;

    protected void Awake()
    {
        ///
        var camera = gameObject.GetComponent<Camera>();
        camera.cullingMask = 0;
        camera.clearFlags = CameraClearFlags.Nothing;
        camera.enabled = true;

        ///
        StartCoroutine(EndOfFrameExecutioner());
    }

    public void ExecuteOnUpdate(Action action)
    {
        OnUpdate += action;
    }

    public void ExecuteOnLateUpdate(Action action)
    {
        OnLateUpdate += action;
    }

    public void ExecuteOnEndOfFrame(Action action)
    {
        OnEndOfFrame += action;
    }

    public void ExecuteOnPreCull(Action action)
    {
        OnPreCullEvent += action;
    }

    protected void Update()
    {
        ///
        OnUpdate?.Invoke();

        ///
        OnUpdate = null;
    }

    protected void LateUpdate()
    {
        ///
        OnLateUpdate?.Invoke();

        ///
        OnLateUpdate = null;
    }

    protected void OnPreCull()
    {
        ///
        OnPreCullEvent?.Invoke();

        ///
        OnPreCullEvent = null;
    }

    private IEnumerator EndOfFrameExecutioner()
    {
        while (true)
        {
            ///
            yield return new WaitForEndOfFrame();

            ///
            OnEndOfFrame?.Invoke();

            ///
            OnEndOfFrame = null;
        }
    }
}
