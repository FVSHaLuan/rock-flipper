using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class ScaleBreather : MonoBehaviour
{
    [SerializeField]
    private float minScale = 0.9f;
    [SerializeField]
    private float maxScale = 1.1f;
    [SerializeField]
    private float minToMaxDuration = 0.2f;

    protected void OnEnable()
    {
        StartCoroutine(MoveToMin());
    }

    protected void OnDisable()
    {
        StopAllCoroutines();
    }

    private IEnumerator MoveToMin()
    {
        //
        yield return StartCoroutine(MoveTo(minScale));

        ///
        StartCoroutine(MoveToMax());
    }

    private IEnumerator MoveToMax()
    {
        //
        yield return StartCoroutine(MoveTo(maxScale));

        ///
        StartCoroutine(MoveToMin());
    }

    private IEnumerator MoveTo(float targetScale)
    {
        ///
        var startScale = Mathf.Clamp(transform.localScale.x, minScale, maxScale);
        var speed = Mathf.Abs(maxScale - minScale) / minToMaxDuration;
        var duration = Mathf.Abs(startScale - targetScale) / speed;

        ///
        float t = 0;

        ///
        while (t < duration)
        {
            ///
            t += Time.deltaTime;

            ///
            var scale = Mathf.Lerp(startScale, targetScale, Mathf.Clamp01(t / duration));
            transform.localScale = Vector3.one * scale;

            ///
            yield return null;
        }
    }
}
