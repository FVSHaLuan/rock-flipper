using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class RandomLocalRotationSetter : MonoBehaviour
{
    [SerializeField]
    private bool setOnEnable = false;

    [Space]
    [SerializeField]
    private Vector3 minRotation;
    [SerializeField]
    private Vector3 maxRotation;

    protected void OnEnable()
    {
        if (setOnEnable)
        {
            SetRandomRotation();
        }
    }

    public void SetRandomRotation()
    {
        ///
        var rotation = new Vector3()
        {
            x = Random.Range(minRotation.x, maxRotation.x),
            y = Random.Range(minRotation.y, maxRotation.y),
            z = Random.Range(minRotation.z, maxRotation.z),
        };

        ///
        transform.localEulerAngles = rotation;
    }
}
