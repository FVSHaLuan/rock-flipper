using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public static class UIImageExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="image"></param>
    /// <param name="fillingAngle">0: no fill, 360: fully filled</param>
    /// <param name="rotatingAngle"></param>
    public static void FillWorldSpaceRadically(this Image image, float fillingAngle, float rotatingAngle)
    {
        ///
        image.fillOrigin = (int)Image.Origin360.Right;
        image.fillMethod = Image.FillMethod.Radial360;
        image.fillClockwise = false;

        ///
        image.fillAmount = fillingAngle / 360.0f;
        image.transform.localEulerAngles = new Vector3(0, 0, -fillingAngle / 2.0f + rotatingAngle);
    }
}
