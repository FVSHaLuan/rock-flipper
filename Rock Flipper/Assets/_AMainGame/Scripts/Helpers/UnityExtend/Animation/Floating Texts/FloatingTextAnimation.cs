using System.Collections;
using FH.Core.Architecture.Pool;
using TMPro;
using UnityEngine;

/// <summary>
/// on enabled, animation starts from the gameobject's current position
/// </summary>
public class FloatingTextAnimation : MonoBehaviour
{
    [SerializeField, Tooltip("scale from 0 to 1")]
    private float appearDuration = 0.2f;
    [SerializeField, Tooltip("stay")]
    private float idleDuration = 0.1f;
    [SerializeField, Tooltip("move up, wiggle left and right, fade alpha to 0")]
    private float exitDuration = 0.5f;
    [SerializeField]
    private float yDistance = 1.0f;
    [SerializeField]
    private float xMaxDelta = 0.1f;
    [SerializeField, Tooltip("number of full left-right wiggles during the exit phase")]
    private float wiggleFrequency = 2.0f;

    [Space]
    [SerializeField]
    private TextMeshPro textMeshPro;
    [SerializeField]
    private GeneralPoolMemberSimplified poolMember;

    private Vector3 startLocalPosition;
    private Color startColor;
    protected void OnEnable()
    {
        startLocalPosition = transform.localPosition;

        startColor = textMeshPro.color;
        startColor.a = 1.0f;
        textMeshPro.color = startColor;

        transform.localScale = Vector3.zero;

        StartCoroutine(Animate());
    }

    private IEnumerator Animate()
    {
        yield return Appear();
        yield return new WaitForSeconds(idleDuration);
        yield return Exit();

        if (poolMember != null)
        {
            poolMember.TryReturnToPoolAndDeactivate();
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private IEnumerator Appear()
    {
        var time = 0.0f;

        while (time < appearDuration)
        {
            time += Time.deltaTime;

            var t = Mathf.Clamp01(time / appearDuration);
            transform.localScale = Vector3.one * t;

            yield return null;
        }

        transform.localScale = Vector3.one;
    }

    private IEnumerator Exit()
    {
        var time = 0.0f;

        while (time < exitDuration)
        {
            time += Time.deltaTime;

            var t = Mathf.Clamp01(time / exitDuration);

            var xOffset = Mathf.Sin(t * wiggleFrequency * Mathf.PI * 2.0f) * xMaxDelta;
            var yOffset = yDistance * t;
            transform.localPosition = startLocalPosition + new Vector3(xOffset, yOffset, 0.0f);

            var color = startColor;
            color.a = Mathf.Lerp(startColor.a, 0.0f, t);
            textMeshPro.color = color;

            yield return null;
        }

        transform.localPosition = startLocalPosition + new Vector3(0.0f, yDistance, 0.0f);

        var finalColor = startColor;
        finalColor.a = 0.0f;
        textMeshPro.color = finalColor;
    }
}
