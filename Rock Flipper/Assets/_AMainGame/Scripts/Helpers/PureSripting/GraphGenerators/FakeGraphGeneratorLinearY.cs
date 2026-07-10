using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeGraphGeneratorLinearY : FakeGraphGeneratorForY
{
    [SerializeField]
    private float minY;
    [SerializeField]
    private float maxY = 5;

    [Space]
    [SerializeField, Min(0)]
    private float minSpeedChangeInterval;
    [SerializeField, Min(0)]
    private float maxSpeedChangeInterval;

    private float lastY;
    private float targetY;
    private float currentSpeedChangeInterval = -1;
    private float timeAccount;

    protected override float UpdateNewY(float deltaTime)
    {
        ///
        if (currentSpeedChangeInterval < 0 || timeAccount >= currentSpeedChangeInterval)
        {
            ///
            ChangeSpeed();
        }

        ///
        timeAccount += deltaTime;

        ///
        return Mathf.Lerp(lastY, targetY, timeAccount / currentSpeedChangeInterval);
    }

    private void ChangeSpeed()
    {
        currentSpeedChangeInterval = Random.Range(minSpeedChangeInterval, maxSpeedChangeInterval);
        lastY = targetY;
        targetY = Random.Range(minY, maxY);
        timeAccount = 0;
    }
}
