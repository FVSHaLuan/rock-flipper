using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FakeGraphGeneratorForY : FakeGraphGenerator
{
    [SerializeField]
    private float speedX = 5;

    private float currentX;

    protected abstract float UpdateNewY(float deltaTime);

    protected sealed override Vector2 UpdateNewPoint(float deltaTime)
    {
        ///
        if (Points.Count == 0)
        {
            currentX = 0;
        }
        else
        {
            currentX += deltaTime * speedX;
        }

        ///
        return new Vector2()
        {
            x = currentX,
            y = UpdateNewY(deltaTime),
        };
    }
}
