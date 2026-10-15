using UnityEngine;

/// <summary>
/// Sets the saturation of a <see cref="SpriteRenderer"/>'s tint color while keeping its hue, value and alpha.
/// Animate <see cref="saturation"/> from an AnimationClip, since clips can only key the color's RGBA channels
/// </summary>
[ExecuteInEditMode]
public class SpriteRendererSaturation : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField, Range(0f, 1f), Tooltip("Multiplies the base color's saturation: 0 is gray, 1 is the original color")]
    private float saturation = 1f;

    protected void Reset()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        Color.RGBToHSV(spriteRenderer.color, out _, out saturation, out _);
    }

    protected void LateUpdate()
    {
        var baseColor = spriteRenderer.color;
        Color.RGBToHSV(baseColor, out var h, out var s, out var v);
        spriteRenderer.color = Color.HSVToRGB(h, saturation, v).OverrideAlpha(baseColor.a);
    }
}
