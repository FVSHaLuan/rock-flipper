using FH.Core.Architecture;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScaleTo : MonoBehaviour
{
    [SerializeField]
    private Transform targetTransform;
    [SerializeField]
    private Vector3 targetScale;
    [SerializeField]
    private float duration = 0.2f;
    [SerializeField]
    private float maxAdditionalDuration;
    [SerializeField]
    private float delay;

    [Space]
    [SerializeField]
    private OrderedEventDispatcher onFinished;

    private int startScaleCalledFrameCount = -1;

    [ContextMenu("StartScale")]
    public void StartScale()
    {
        if (isActiveAndEnabled)
        {
            StopAllCoroutines();
            StartCoroutine(Scale());
        }
        else
        {
            startScaleCalledFrameCount = Time.frameCount;
        }
    }

    protected void OnEnable()
    {
        if (startScaleCalledFrameCount == Time.frameCount)
        {
            StartScale();
        }
    }

    private IEnumerator Scale()
    {
        ///
        if (!Mathf.Approximately(delay, 0))
        {
            yield return new WaitForSeconds(delay);
        }

        ///
        var time = 0.0f;
        var startScale = targetTransform.localScale;

        ///
        var effectiveDuration = duration + Random.Range(0, maxAdditionalDuration);

        ///
        while (time <= effectiveDuration)
        {
            ///
            time += Time.deltaTime;

            ///
            var currentScale = Vector3.Lerp(startScale, targetScale, time / effectiveDuration);
            targetTransform.localScale = currentScale;

            ///
            yield return null;
        }

        ///
        onFinished?.Dispatch();
    }

#if UNITY_EDITOR
    public void Reset()
    {
        targetTransform = transform;
    }
#endif
}
