using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FH.Core.Architecture;
using UnityEngine.Events;
using UnityEngine.Serialization;

/// <summary>
/// 
/// </summary>
/// <typeparam name="T">property's type</typeparam>
public abstract class PropertyMover<T> : MonoBehaviour
{
    [Header("PropertyMover")]
    [SerializeField]
    private T targetValue;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float minAdditionalSpeed = 0;
    [SerializeField, FormerlySerializedAs("maxnAdditionalSpeed")]
    private float maxAdditionalSpeed = 0;
    [SerializeField]
    private bool useUnscaledTime;
    [SerializeField]
    private TimeScaleMode gameplayTimeScaleMode = TimeScaleMode.GameplayUnscaledTime;

    [Space]
    [SerializeField]
    private UnityEvent onFinished;

    public float Speed { get; set; }
    public bool IsMoving { get; private set; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="deltaTime"></param>
    /// <returns>true if after moved reached the target</returns>
    protected abstract bool MoveToTarget(T targetValue, float deltaTime);

    protected virtual void OnDisable()
    {
        StopAllCoroutines();
        IsMoving = false;
    }

    [ContextMenu("MoveToTargetValue")]
    public void MoveToTargetValue()
    {
        MoveTo(targetValue);
    }

    public void MoveTo(T targetValue)
    {
        StopAllCoroutines();
        StartCoroutine(Move(targetValue));
    }

    public void Stop()
    {
        IsMoving = false;
        StopAllCoroutines();
    }

    private IEnumerator Move(T targetValue)
    {
        ///
        bool isDone = false;
        IsMoving = true;

        ///
        Speed = speed + Random.Range(minAdditionalSpeed, maxAdditionalSpeed);

        ///        
        while (!isDone)
        {
            ///
            float deltaTime = GetDeltaTime();
            isDone = MoveToTarget(targetValue, deltaTime);

            ///
            yield return null;
        };

        ///
        onFinished?.Invoke();

        ///
        IsMoving = false;
    }

    private float GetDeltaTime()
    {
        if (!useUnscaledTime)
        {
            return Time.deltaTime;
        }

        ///
        if (Entry.Instance == null)
        {
            return Time.unscaledTime;
        }

        ///
        return Entry.Instance.timeScaleManager.GetDeltaTime(gameplayTimeScaleMode);
    }
}
