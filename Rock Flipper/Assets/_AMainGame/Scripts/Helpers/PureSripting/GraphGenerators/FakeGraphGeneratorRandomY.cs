using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeGraphGeneratorRandomY : FakeGraphGeneratorForY
{
    [SerializeField]
    private float minY;
    [SerializeField]
    private float maxY = 5;

    protected override float UpdateNewY(float deltaTime)
    {
        return Random.Range(minY, maxY);
    }
}
