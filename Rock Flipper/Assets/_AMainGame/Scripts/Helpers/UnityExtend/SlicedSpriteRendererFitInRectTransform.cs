using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(SpriteRenderer))]
public class SlicedSpriteRendererFitInRectTransform : MonoBehaviour
{
    [SerializeField]
    private RectTransform parentRectTransform;
    [SerializeField]
    private Vector2 positionModifier;

    private SpriteRenderer spriteRenderer;

    protected void Start()
    {
        ///
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected void LateUpdate()
    {
        UpdateSpriteRenderer();
    }

    private void UpdateSpriteRenderer()
    {
        ///
        var size = parentRectTransform.sizeDelta;

        ///
        spriteRenderer.size = size;

        ///
        transform.localPosition = new Vector2()
        {
            x = positionModifier.x * size.x,
            y = positionModifier.y * size.y
        };
    }

#if UNITY_EDITOR
    protected void Reset()
    {
        GetComponent<SpriteRenderer>().drawMode = SpriteDrawMode.Sliced;
    }
#endif

}
