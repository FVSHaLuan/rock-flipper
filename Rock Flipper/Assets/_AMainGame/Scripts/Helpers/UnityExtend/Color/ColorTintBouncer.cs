using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UnifiedColoredObject))]
public class ColorTintBouncer : MonoBehaviour
{
    [SerializeField]
    private Color tintColor;
    [SerializeField]
    private float recoverSpeed = 3;
    [SerializeField, Range(0, 1)]
    private float maxAcceptantTintAmount = 0;

    [Space]
    [SerializeField]
    private bool overrideMode = false;
    [SerializeField]
    private Color originalColor;

    private bool isRecovering = false;

    protected bool IsRecovering
    {
        get => isRecovering;
        set
        {
            isRecovering = value;
        }

    }
    private float tintAmount;
    private UnifiedColoredObject unifiedColoredObject;

    public Color OriginalColor
    {
        get => originalColor;
        set
        {
            ///
            originalColor = value;

            ///
            UpdateTint(tintAmount);
        }
    }

    protected void OnEnable()
    {
        if (!IsRecovering)
        {
            tintAmount = 0;
            UpdateTint(tintAmount);
        }
    }

    protected void OnDisable()
    {
        tintAmount = 0;
        IsRecovering = false;
    }

    private void UpdateTint(float tintAmount)
    {
        ///
        if (unifiedColoredObject == null)
        {
            unifiedColoredObject = GetComponent<UnifiedColoredObject>();
        }

        ///
        if (!overrideMode)
        {
            var effectiveTintColor = Color.Lerp(Color.white, tintColor, tintAmount);

            ///
            unifiedColoredObject.Tint = effectiveTintColor;
        }
        else
        {
            var overrideColor = Color.Lerp(originalColor, tintColor, tintAmount);
            unifiedColoredObject.Color = overrideColor;
        }
    }

    [ContextMenu("Tint")]
    public void Tint()
    {
        ///
        if (IsRecovering && tintAmount >= maxAcceptantTintAmount)
        {
            return;
        }

        ///
        tintAmount = 1;
        UpdateTint(1);
        IsRecovering = true;
    }

    protected virtual void Update()
    {

        ///
        if (!IsRecovering)
        {
            return;
        }

        ///
        tintAmount -= recoverSpeed * Time.deltaTime;
        if (tintAmount <= 0)
        {
            tintAmount = 0;
            IsRecovering = false;
        }

        ///
        UpdateTint(tintAmount);
    }
}
