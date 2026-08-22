using UnityEngine;

public class PositionLooper : MonoBehaviour
{
    [SerializeField]
    private AnimationCurve xCurve;
    [SerializeField]
    private AnimationCurve yCurve;
    [SerializeField]
    private float loopDuration = 1f;
    [SerializeField]
    private bool isLocalPosition = true;
    [SerializeField]
    private bool startAtRandomOffset = false;
    [SerializeField, Tooltip("When enabled, plays the curve forward then backward instead of jumping back to the start")]
    private bool pingPong = false;
    [SerializeField, Tooltip("Multiplies the evaluated curve values, scaling the loop's amplitude")]
    private Vector2 curveScale = Vector2.one;

    private Vector3 basePosition;
    private float timeOffset;

    protected void OnEnable()
    {
        basePosition = GetPosition();
        timeOffset = startAtRandomOffset ? Random.Range(0f, loopDuration) : 0f;
    }

    protected void OnDisable()
    {
        SetPosition(basePosition);
    }

    protected void Update()
    {
        if (loopDuration <= 0f)
        {
            return;
        }

        var elapsed = Time.time + timeOffset;
        var t = (pingPong
            ? Mathf.PingPong(elapsed, loopDuration)
            : Mathf.Repeat(elapsed, loopDuration)) / loopDuration;
        var offset = new Vector3(xCurve.Evaluate(t) * curveScale.x, yCurve.Evaluate(t) * curveScale.y, 0f);

        SetPosition(basePosition + offset);
    }

    private Vector3 GetPosition()
    {
        return isLocalPosition ? transform.localPosition : transform.position;
    }

    private void SetPosition(Vector3 position)
    {
        if (isLocalPosition)
        {
            transform.localPosition = position;
        }
        else
        {
            transform.position = position;
        }
    }
}
