using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Graphic))]
public class UIGraphicsColoredObject : UnifiedColoredObject
{
    protected override void SetColor(Color color)
    {
        GetComponent<Graphic>().color = color;
    }
}
