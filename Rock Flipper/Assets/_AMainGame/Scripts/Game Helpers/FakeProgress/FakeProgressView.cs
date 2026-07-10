using GD;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FakeProgressView : ExtendedMonoBehaviourWithTime
{
    [SerializeField]
    private FakeProgress fakeProgress;
    [SerializeField]
    private ProgressBar progressBar;

    [Space]
    [SerializeField]
    private float progressScale = 1;
    [SerializeField]
    private float minProgressScale = 1;
    [SerializeField]
    private float maxProgressScale = 1;

    [Space]
    [SerializeField]
    private bool resetFakeProgressOnEnable = true;
    [SerializeField]
    private bool looping = false;

    [Space]
    [SerializeField]
    private float minTapValue = 0;
    [SerializeField]
    private float maxTapValue = 0.1f;

    [Space]
    [SerializeField]
    private UnityEvent onFinishedProgress;

    private bool calledFinishedProgress;

    public float SpeedScale { get => fakeProgress.SpeedScale; set => fakeProgress.SpeedScale = value; }
    public float MinProgressScale { get => minProgressScale; set => minProgressScale = value; }
    public float MaxProgressScale { get => maxProgressScale; set => maxProgressScale = value; }

    protected void OnEnable()
    {
        ///
        if (resetFakeProgressOnEnable)
        {
            ResetFakeProgress();
        }

        ///
        CommonCheatLib.OnCheatSignalEmitted += CommonCheatLib_OnCheatSignalEmitted;
    }

    protected void OnDisable()
    {
        ///
        CommonCheatLib.OnCheatSignalEmitted -= CommonCheatLib_OnCheatSignalEmitted;
    }

    private void CommonCheatLib_OnCheatSignalEmitted(string obj)
    {
        fakeProgress.AddValue(1);
    }

    public void ResetFakeProgress()
    {
        ///
        fakeProgress.Reset(Random.Range(int.MinValue, int.MaxValue));

        ///
        progressScale = Random.Range(MinProgressScale, MaxProgressScale);

        ///
        calledFinishedProgress = false;
    }

    protected void Update()
    {
        ///
        if (!fakeProgress.Inited)
        {
            return;
        }

        ///
        fakeProgress.Update(GameplayDeltaTime);

        ///
        progressBar.SetValue(fakeProgress.Progress * progressScale);

        ///
        if (Mathf.Approximately(fakeProgress.Progress, 1))
        {
            ///
            if (!calledFinishedProgress)
            {
                ///
                calledFinishedProgress = true;

                ///
                onFinishedProgress?.Invoke();
            }

            ///
            if (looping)
            {
                ResetFakeProgress();
            }
        }
    }

    public void Tap()
    {
        ///
        var value = Random.Range(minTapValue, maxTapValue);

        ///
        fakeProgress.AddValue(value * SpeedScale);
    }
}
