using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class ConcurrentActivator : MonoBehaviourWithInit
{
    [SerializeField]
    private float minInterval = 0;
    [SerializeField]
    private int maxPerFrame = 10000;

    [Space]
    [SerializeField]
    private UnityEvent onShouldActivate;
    [SerializeField]
    private UnityEvent onShouldNotActivate;

    protected abstract ConcurrentActivationManager.IKeyState GetKeyState();
    protected abstract void UpdateWithActivation();

    public void TryActivate()
    {
        TryActivateWithResult();
    }

    public bool TryActivateWithResult()
    {
        ///
        var ks = GetKeyState();

        ///
        if (ks.ActivationCountThisFrame >= maxPerFrame)
        {
            ///
            SignalShouldNotActivate();

            ///
            return false;
        }

        ///
        if ((ks.LastActivationTime >= 0) && (Time.realtimeSinceStartup - ks.LastActivationTime <= minInterval))
        {
            ///
            SignalShouldNotActivate();

            ///
            return false;
        }

        ///
        SignalShouldActivate();
        UpdateWithActivation();

        ///
        return true;
    }

    private void SignalShouldNotActivate()
    {
        onShouldNotActivate?.Invoke();
    }

    private void SignalShouldActivate()
    {
        onShouldActivate?.Invoke();
    }
}
