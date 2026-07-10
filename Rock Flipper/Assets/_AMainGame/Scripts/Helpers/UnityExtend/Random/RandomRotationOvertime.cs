using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRotationOvertime : MonoBehaviour
{
    [SerializeField]
    private float minAngle = 0;
    [SerializeField]
    private float maxAngle = 180;

    [Space]
    [SerializeField]
    private float minAngularSpeed = 20;
    [SerializeField]
    private float maxAngularSpeed = 180;

    [Space]
    [SerializeField]
    private bool isLocal = true;

    [Space]
    [SerializeField]
    private bool alwaysClamp = false;

    private float angleZ;

    private float AngleZ
    {
        get
        {
            return angleZ;
        }

        set
        {
            ///
            angleZ = value;

            ///
            if (isLocal)
            {
                var e = transform.localEulerAngles;
                e.z = value;
                transform.localEulerAngles = e;
            }
            else
            {
                var e = transform.eulerAngles;
                e.z = value;
                transform.eulerAngles = e;
            }
        }
    }

    protected void OnEnable()
    {
        ///
        AngleZ = Mathf.Clamp(AngleZ, minAngle, maxAngle);

        ///
        StartCoroutine(RotationLoopRoutine());
    }

    protected void OnDisable()
    {
        StopAllCoroutines();
    }

    private IEnumerator RotationLoopRoutine()
    {
        while (true)
        {
            ///
            float targetAngle = Random.Range(minAngle, maxAngle);
            float duration;

            ///
            float angularSpeed = Random.Range(minAngularSpeed, maxAngularSpeed);
            var currentAngle = AngleZ;
            duration = Mathf.Abs(currentAngle - targetAngle) / angularSpeed;

            ///
            yield return StartCoroutine(RotationSingleRoutine(targetAngle, duration));
        }
    }


    private IEnumerator RotationSingleRoutine(float targetAngle, float duration)
    {
        ///
        float t = 0.0f;

        ///
        var startAngle = AngleZ;

        ///
        while (t <= duration)
        {
            ///
            var deltaTime = Time.deltaTime;

            ///
            var effectiveStartAngle = startAngle;
            var effectiveTargetAngle = targetAngle;

            ///
            if (alwaysClamp)
            {
                effectiveStartAngle = Mathf.Clamp(effectiveStartAngle, minAngle, maxAngle);
                effectiveTargetAngle = Mathf.Clamp(effectiveTargetAngle, minAngle, maxAngle);
            }

            ///
            t += deltaTime;

            ///
            var currentAngle = Mathf.Lerp(effectiveStartAngle, effectiveTargetAngle, t / duration);

            ///
            AngleZ = currentAngle;

            ///
            yield return null;
        }
    }
}
