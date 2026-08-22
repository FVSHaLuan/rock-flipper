using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class ObjectSpeedometer : MonoBehaviour
{
    public Vector3 InstantVelocity { get; private set; }
    public Vector3 LastPosition { get; private set; }

    public void OnEnable()
    {
        LastPosition = transform.position;
    }

    public void LateUpdate()
    {
        UpdateInstantVelocity();
    }

    private void UpdateInstantVelocity()
    {
        ///
        var currentPos = transform.position;

        ///
        if (!Mathf.Approximately(Time.deltaTime, 0))
        {
            InstantVelocity = (currentPos - LastPosition) / Time.deltaTime;
        }

        ///
        LastPosition = transform.position;
    }
}
