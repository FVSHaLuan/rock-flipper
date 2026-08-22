using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleBouncer : MonoBehaviour
{
    [SerializeField]
    private float normalScale = 1;
    [SerializeField]
    private float recoverSpeed = 3;
    [SerializeField, Tooltip("Ignore SetScale when current scale is that far or further from the normalScale")]
    private float maxAcceptantDeltaScale = 0;

    private bool isRecovering = false;

    protected void OnEnable()
    {
        if (!isRecovering)
        {
            UpdateScale(normalScale);
        }
    }

    public void SetScale(float scale)
    {
        ///
        if (isRecovering && Mathf.Abs(transform.localScale.x - normalScale) >= maxAcceptantDeltaScale)
        {
            return;
        }

        ///
        UpdateScale(scale);
        isRecovering = true;
    }

    private void UpdateScale(float scale)
    {
        transform.localScale = Vector3.one * scale;
    }

    protected void Update()
    {
        ///
        if (!isRecovering)
        {
            return;
        }

        ///
        var scaleDelta = recoverSpeed * Time.deltaTime;
        var scale = Mathf.MoveTowards(transform.localScale.x, normalScale, scaleDelta);

        ///
        if (Mathf.Approximately(scale, normalScale))
        {
            ///
            isRecovering = false;

            ///
            scale = normalScale;
        }

        ///
        UpdateScale(scale);
    }
}
