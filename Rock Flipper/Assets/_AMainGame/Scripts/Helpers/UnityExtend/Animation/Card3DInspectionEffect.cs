using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card3DInspectionEffect : MonoBehaviour
{
    private readonly Vector2 stillInput = new Vector2(0.5f, 0.5f);

    [SerializeField]
    private Camera conversionCamera;
    [SerializeField]
    private float maxXRotation = 10;
    [SerializeField]
    private float maxYRotation = 10;
    [SerializeField]
    private float maxDeltaX = 2;
    [SerializeField]
    private float maxDeltaY = 2;

    [Space]
    [SerializeField]
    private float animationDuration = 0.5f;
    [SerializeField]
    private float animationAmplitude = 0.5f;
    [SerializeField]
    private float animationFrequency = 5;
    [SerializeField]
    private float animationBreakingMagnitude = 0.1f;

    [Space]
    [SerializeField]
    private bool editor_Debug;

    private bool isAnimating = false;

    protected Vector2 StillInput => stillInput;

    protected void Update()
    {
        ///
        var normalizedInput = UpdateNormalizedInput();

        ///
        if (isAnimating)
        {
            // Logic to break animation
            if ((normalizedInput - stillInput).sqrMagnitude >= (animationBreakingMagnitude * animationBreakingMagnitude))
            {
                StopAllCoroutines();
                isAnimating = false;
            }

            ///
            return;
        }

        ///
        SetRotation(normalizedInput.x, normalizedInput.y);
    }

    protected virtual Vector2 UpdateNormalizedInput()
    {
        return GetNormalizedInputByMouse();
    }

    protected Vector2 GetNormalizedInputByMouse()
    {
        if (conversionCamera == null)
        {
            return StillInput;
        }

        ///
        var mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = transform.position.z - conversionCamera.transform.position.z;
        var mouseWorldPos = conversionCamera.ScreenToWorldPoint(mouseScreenPos);
        var deltaX = mouseWorldPos.x - transform.position.x;
        var deltaY = mouseWorldPos.y - transform.position.y;

        ///
#if UNITY_EDITOR
        if (editor_Debug)
        {
            Debug.LogFormat("{0} - {1} - {2}", Input.mousePosition, mouseWorldPos, transform.position);
        }
#endif

        ///
        return new Vector2((deltaX + maxDeltaX) / (2 * maxDeltaX), (deltaY + maxDeltaY) / (2 * maxDeltaY));
    }

    protected void SetRotation(float normalizedX, float normalizedY)
    {
        var yRotation = Mathf.Lerp(maxYRotation, -maxYRotation, normalizedX);
        var xRotation = Mathf.Lerp(-maxXRotation, maxXRotation, normalizedY);

        ///
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0);
    }

    public void DisableAndResetRotation()
    {
        ///
        if (this == null)
        {
            return;
        }

        ///
        enabled = false;
        transform.rotation = Quaternion.identity;
    }

    [ContextMenu("Animate")]
    public virtual void Animate()
    {
        ///
        StopAllCoroutines();

        ///
        if (gameObject.activeInHierarchy)
        {
            StartCoroutine(AnimateCoroutine());
        }
        else
        {
            Debug.LogWarning("Can't run animate coroutine, gameObject isn't active");
        }
    }

    private IEnumerator AnimateCoroutine()
    {
        float t = 0;

        ///
        isAnimating = true;

        ///
        while (t <= animationDuration)
        {
            ///
            t += Time.unscaledDeltaTime;

            ///
            float magnitude = Mathf.Lerp(animationAmplitude, 0, t);
            float angle = Mathf.Repeat(animationFrequency / animationDuration * t, 1) * Mathf.PI * 2;

            ///
            var v = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * magnitude;

            ///
            v.x = Mathf.InverseLerp(-1, 1, v.x);
            v.y = Mathf.InverseLerp(-1, 1, v.y);

            ///
            SetRotation(v.x, v.y);

            ///
            yield return null;
        }

        ///
        isAnimating = false;
    }
}
