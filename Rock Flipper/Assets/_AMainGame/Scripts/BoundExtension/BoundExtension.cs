using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BT
{
    [System.Serializable]
    public struct BoundExtension
    {
        public float up;
        public float down;
        public float left;
        public float right;

        public float Width => left + right;
        public float Height => up + down;

        public static BoundExtension Zero => new BoundExtension() { up = 0, down = 0, left = 0, right = 0 };

        public static BoundExtension Lerp(BoundExtension a, BoundExtension b, float t)
        {
            return new BoundExtension()
            {
                up = Mathf.Lerp(a.up, b.up, t),
                down = Mathf.Lerp(a.down, b.down, t),
                left = Mathf.Lerp(a.left, b.left, t),
                right = Mathf.Lerp(a.right, b.right, t),
            };
        }

        public static BoundExtension Clamp(BoundExtension bound, BoundExtension min, BoundExtension max)
        {
            return new BoundExtension()
            {
                up = Mathf.Clamp(bound.up, min.up, max.up),
                down = Mathf.Clamp(bound.down, min.down, max.down),
                left = Mathf.Clamp(bound.left, min.left, max.left),
                right = Mathf.Clamp(bound.right, min.right, max.right),
            };
        }

        public override string ToString()
        {
            return string.Format("({0}, {1}, {2}, {3})", up, down, left, right);
        }

        public BoundExtension SquareUp()
        {
            ///
            var width = Width;
            var height = Height;

            ///
            if (Mathf.Approximately(width, height))
            {
                return this;
            }

            ///
            if (width > height)
            {
                return ExtendHeight(width - height);
            }
            else
            {
                return ExtendWidth(height - width);
            }
        }

        /// <summary>
        /// extend height balancedly from visual center
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public BoundExtension ExtendHeight(float amount)
        {
            ///
            var rs = this;
            var halfAmount = amount / 2.0f;
            rs.up += halfAmount;
            rs.down += halfAmount;

            ///
            return rs;
        }

        /// <summary>
        /// extend width balancedly from visual center
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public BoundExtension ExtendWidth(float amount)
        {
            ///
            var rs = this;
            var halfAmount = amount / 2.0f;
            rs.left += halfAmount;
            rs.right += halfAmount;

            ///
            return rs;
        }

        public BoundExtension ExtendChooseMax(BoundExtension boundExtension)
        {
            return Extend(boundExtension, BoundAddUpOption.ChooseMax, BoundAddUpOption.ChooseMax, BoundAddUpOption.ChooseMax, BoundAddUpOption.ChooseMax);
        }

        public BoundExtension ExtendAddictive(BoundExtension boundExtension)
        {
            return Extend(boundExtension, BoundAddUpOption.Addictive, BoundAddUpOption.Addictive, BoundAddUpOption.Addictive, BoundAddUpOption.Addictive);
        }

        public BoundExtension Extend(BoundExtension boundExtension, BoundAddUpOptions options)
        {
            return Extend(boundExtension, options.upOption, options.downOption, options.leftOption, options.rightOption);
        }

        public BoundExtension Extend(IBoundExtensionObject boundExtensionObject)
        {
            return Extend(boundExtensionObject.BoundExtension, boundExtensionObject.AddUpOptions);
        }

        public BoundExtension Extend(BoundExtension boundExtension, BoundAddUpOption upOption, BoundAddUpOption downOption, BoundAddUpOption leftOption, BoundAddUpOption rightOption)
        {
            ///
            return new BoundExtension()
            {
                up = AddComponent(up, boundExtension.up, upOption),
                down = AddComponent(down, boundExtension.down, downOption),
                left = AddComponent(left, boundExtension.left, leftOption),
                right = AddComponent(right, boundExtension.right, rightOption),
            };
        }

        private float AddComponent(float valueA, float valueB, BoundAddUpOption option)
        {
            switch (option)
            {
                case BoundAddUpOption.Addictive:
                    return valueA + valueB;
                case BoundAddUpOption.ChooseMax:
                    return Mathf.Max(valueA, valueB);
                case BoundAddUpOption.Ignore:
                    return valueA;
                case BoundAddUpOption.Multiplying:
                    return valueA * valueB;
                default:
                    throw new System.NotImplementedException();
            }
        }

        public void GetCorners(Vector2 extensionCenter, Vector2 upVector, Vector2 rightVector, out Vector2 bottomLeft, out Vector2 bottomRight, out Vector2 topLeft, out Vector2 topRight)
        {
            bottomLeft = extensionCenter - upVector * down - rightVector * left;
            bottomRight = extensionCenter - upVector * down + rightVector * right;
            topLeft = extensionCenter + upVector * up - rightVector * left;
            topRight = extensionCenter + upVector * up + rightVector * right;
        }
    }

}