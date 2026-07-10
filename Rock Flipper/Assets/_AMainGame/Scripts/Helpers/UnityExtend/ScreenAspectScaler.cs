using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenAspectScaler : MonoBehaviour
{
    [SerializeField]
    private Axis fixedAxis;

    public enum Axis
    {
        X, Y
    }

    public void OnEnable()
    {
        Apply();
    }

    public void Apply()
    {
        ///
        var scale = transform.localScale;

        ///
        switch (fixedAxis)
        {
            case Axis.X:
                scale.y = Screen.height / (float)Screen.width * scale.x;
                break;
            case Axis.Y:
                scale.x = Screen.width / (float)Screen.height * scale.y;
                break;
            default:
                throw new System.NotImplementedException();
        }

        ///
        transform.localScale = scale;
    }
}
