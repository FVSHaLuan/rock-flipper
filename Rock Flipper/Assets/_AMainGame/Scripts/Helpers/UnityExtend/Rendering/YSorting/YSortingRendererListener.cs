using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(Renderer))]
public class YSortingRendererListener : YSortingListener
{
    private SpriteRenderer renderer;

    private SpriteRenderer Renderer
    {
        get
        {
            if (renderer == null)
            {
                renderer = GetComponent<SpriteRenderer>();
            }
            return renderer;
        }
    }

    public override int SortingLayer => Renderer.sortingLayerID;

    public override int SortingOrder
    {
        get => Renderer.sortingOrder;
        set => Renderer.sortingOrder = value;
    }

    public override float Y => Renderer.transform.position.y;
}
