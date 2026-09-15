using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GD;

[DisallowMultipleComponent, RequireComponent(typeof(Image))]
public class ImageRandomSpriteView : OneTimeView
{
    [SerializeField]
    private List<Sprite> sprites;

    private Image image;

    public override void UpdateView()
    {
        ///
        if (image == null)
        {
            image = GetComponent<Image>();
        }

        ///
        image.sprite = sprites.GetRandomItem();
    }
}
