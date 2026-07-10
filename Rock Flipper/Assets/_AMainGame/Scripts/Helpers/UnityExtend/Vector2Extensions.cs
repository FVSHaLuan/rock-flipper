using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BT
{
    public static class Vector2Extensions
    {
        public static Vector2 SquareUp(this Vector2 v)
        {
            ///
            var max = Mathf.Max(v.x, v.y);

            ///
            return new Vector2()
            {
                x = max,
                y = max,
            };
        }
    }

}