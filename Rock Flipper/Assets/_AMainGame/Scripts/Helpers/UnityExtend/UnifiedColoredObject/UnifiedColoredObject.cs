using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public abstract class UnifiedColoredObject : MonoBehaviour
{
    [Header("UnifiedColoredObject")]
    [SerializeField]
    private Color color = Color.white;
    [SerializeField]
    private Color tint = Color.white;

    private bool isSetOnce = false;

    public Color Color
    {
        get => color;
        set
        {
            ///
            color = value;

            ///
            if (!isSetOnce)
            {
                isSetOnce = true;
            }

            ///
            SetColor(GetTintedColor());
        }
    }

    public Color Tint
    {
        get => tint;
        set
        {
            ///
            tint = value;

            ///
            if (!isSetOnce)
            {
                isSetOnce = true;
            }

            ///
            SetColor(GetTintedColor());
        }
    }

    protected abstract void SetColor(Color color);

    public void SetAlpha(float alpha)
    {
        ///
        var color = this.color;
        color.a = alpha;

        ///
        Color = color;
    }

    public void OnEnable()
    {
        if (!isSetOnce)
        {
            Color = color;
        }
    }

    private Color GetTintedColor()
    {
        return new Color
        {
            a = color.a * tint.a,
            b = color.b * tint.b,
            g = color.g * tint.g,
            r = color.r * tint.r
        };
    }

#if UNITY_EDITOR
    [ContextMenu("Editor_SetColor")]
    private void Editor_SetColor()
    {
        SetColor(GetTintedColor());
    }
#endif
}
