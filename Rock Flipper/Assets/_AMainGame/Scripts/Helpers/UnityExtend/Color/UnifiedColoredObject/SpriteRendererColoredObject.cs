using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteRendererColoredObject : UnifiedColoredObject
{
    protected override void SetColor(Color color)
    {
        GetComponent<SpriteRenderer>().color = color;
    }
}
