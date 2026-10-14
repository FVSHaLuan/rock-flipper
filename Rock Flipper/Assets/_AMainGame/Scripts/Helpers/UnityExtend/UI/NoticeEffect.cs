using System.Collections;
using UnityEngine;

/// <summary>
/// A one-shot effect that draws attention to something (e.g. a tab button that just received something).
/// Put it on the effect object itself, which stays inactive until <see cref="Play"/>: it then activates, pops from a
/// bigger scale back to 1 and deactivates again.
/// </summary>
public class NoticeEffect : MonoBehaviour
{
    [SerializeField, Min(0)]
    private float duration = 0.3f;
    [SerializeField, Tooltip("The effect's scale over its duration (0 to 1). Should end at 1.")]
    private AnimationCurve scale = new AnimationCurve(
        new Keyframe(0f, 1.6f, 0f, -4f),
        new Keyframe(0.45f, 0.92f, 0f, 0f),
        new Keyframe(0.75f, 1.04f, 0f, 0f),
        new Keyframe(1f, 1f, 0f, 0f));

    /// <summary>
    /// Plays the effect, restarting it if it's already playing. Does nothing while the effect's parent is inactive.
    /// </summary>
    [ContextMenu("Play")]
    public void Play()
    {
        ///
        gameObject.SetActive(true);
        if (!isActiveAndEnabled)
        {
            gameObject.SetActive(false);
            return;
        }

        ///
        StopAllCoroutines();
        StartCoroutine(Animate());
    }

    protected void OnDisable()
    {
        transform.localScale = Vector3.one;
    }

    private IEnumerator Animate()
    {
        for (var elapsed = 0f; elapsed < duration; elapsed += Time.deltaTime)
        {
            transform.localScale = Vector3.one * scale.Evaluate(elapsed / duration);
            yield return null;
        }

        ///
        gameObject.SetActive(false);
    }
}
