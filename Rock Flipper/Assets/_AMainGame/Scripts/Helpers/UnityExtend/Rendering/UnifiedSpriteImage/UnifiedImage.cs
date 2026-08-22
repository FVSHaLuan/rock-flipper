using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class UnifiedImage : UnifiedSpriteImage
{
    private Image image;

    public Image Image
    {
        get
        {
            ///
            if (image == null)
            {
                image = GetComponent<Image>();
            }

            ///
            return image;
        }
    }

    public override Sprite Sprite
    {
        get => Image.sprite;
        set => Image.sprite = value;
    }

    protected override void SetColor(Color color)
    {
        Image.color = color;
    }
}
