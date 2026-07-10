using GD;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class RandomizeColorComponents : MonoBehaviour
{
    [SerializeField]
    private bool setAtEnabled = true;

    [Space]
    [SerializeField]
    private bool randomizeR = false;
    [SerializeField]
    private bool randomizeG = false;
    [SerializeField]
    private bool randomizeB = false;
    [SerializeField]
    private bool randomizeA = false;

    protected void OnEnable()
    {
        if (setAtEnabled)
        {
            SetRandomColor();
        }
    }

    [ContextMenu("SetRandomColor")]
    public void SetRandomColor()
    {
        ///
        var spriteRenderer = GetComponent<SpriteRenderer>();
        var c = spriteRenderer.color;

        ///
        if (randomizeA)
        {
            c.a = Random.value;
        }
        if (randomizeB)
        {
            c.b = Random.value;
        }
        if (randomizeR)
        {
            c.r = Random.value;
        }
        if (randomizeG)
        {
            c.g = Random.value;
        }

        ///
        spriteRenderer.color = c;
    }
}
