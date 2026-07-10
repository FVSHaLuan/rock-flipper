using FMod;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRandomlyInCircle : MonoBehaviour
{
    [SerializeField]
    private float radius;
    [SerializeField]
    private RandomFloat speed = new RandomFloat(3, 5);

    [Space]
    [SerializeField]
    private bool clampPositionOnEnable = true;

    private Vector2 targetPos;
    private float effectiveSpeed;

    protected void OnEnable()
    {
        ///
        if (clampPositionOnEnable)
        {
            ClampPosition();
        }

        ///
        CalculateNewTargetPosAndEffectiveSpeed();
    }

    private void ClampPosition()
    {
        transform.localPosition = Random.insideUnitCircle * radius;
    }

    protected void Update()
    {
        ///
        var p = transform.localPosition;
        p = Vector3.MoveTowards(p, targetPos, effectiveSpeed * Time.deltaTime);
        transform.localPosition = p;

        ///
        if (Mathf.Approximately(Vector3.SqrMagnitude(p - (Vector3)targetPos), 0))
        {
            CalculateNewTargetPosAndEffectiveSpeed();
        }
    }

    private void CalculateNewTargetPosAndEffectiveSpeed()
    {
        targetPos = Random.insideUnitCircle * radius;
        effectiveSpeed = speed;
    }
}
