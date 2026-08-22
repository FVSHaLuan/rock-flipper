using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class UnifiedSpriteRenderer : UnifiedSpriteImage
{
    private SpriteRenderer spriteRenderer;

    public SpriteRenderer SpriteRenderer
    {
        get
        {
            ///
            if (spriteRenderer == null)
            {
                spriteRenderer = GetComponent<SpriteRenderer>();
            }

            ///
            return spriteRenderer;
        }
    }

    public override Sprite Sprite
    {
        get => SpriteRenderer.sprite;
        set => SpriteRenderer.sprite = value;
    }

    protected override void SetColor(Color color)
    {
        SpriteRenderer.color = color;
    }
}
