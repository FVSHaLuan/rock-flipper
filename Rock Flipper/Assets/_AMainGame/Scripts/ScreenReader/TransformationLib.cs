using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BT.Systems.ScreenReaders
{
    public static class TransformationLib
    {
        /// <summary>
        /// all input positions must be normalized
        /// </summary>
        /// <param name="bottomLeft"></param>
        /// <param name="bottomRight"></param>
        /// <param name="topLeft"></param>
        public static Matrix4x4 GetTetxtcoordMatrix(Vector2 bottomLeft, Vector2 bottomRight, Vector2 topLeft)
        {
            ///
            return _GetTetxtcoordMatrix(bottomLeft, bottomRight, topLeft);
        }

        private static Matrix4x4 _GetTetxtcoordMatrix(Vector2 o, Vector2 a, Vector2 b)
        {
            ///
            var col0 = new Vector4(a.x - o.x, a.y - o.y, 0, 0);
            var col1 = new Vector4(b.x - o.x, b.y - o.y, 0, 0);
            var col2 = new Vector4(0, 0, 1, 0);
            var col3 = new Vector4(o.x, o.y, 0, 1);

            ///
            Matrix4x4 m = new Matrix4x4(col0, col1, col2, col3);

            ///
            return m;
        }
    }
}