using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Agame.Run.Combat
{
    public partial class Playfield : ExtendedMonoBehaviourRun
    {
        public event System.Action OnCalculated;

        [SerializeField]
        private float blowRadius;

        public bool IsCalculated { get; private set; } = false;

        public Vector2 CenterPoint => transform.position;
        public float Width { get; private set; }
        public float Height { get; private set; }
        public float HalfWidth { get; private set; }
        public float HalfHeight { get; private set; }
        public float MinX { get; private set; }
        public float MaxX { get; private set; }
        public float MinY { get; private set; }
        public float MaxY { get; private set; }
        public Vector2 Size { get; private set; }
        public float TapMinFarDistance { get; private set; }
        public float TapMinFarDistanceSqr { get; private set; }

        public Rect PlayfieldRect { get; private set; }

        protected override bool Init()
        {
            Calculate();

            ///
            return base.Init();
        }

        private void Calculate()
        {
            ///
            Height = transform.lossyScale.y;
            Width = transform.lossyScale.x;

            ///
            Size = new Vector2(Width, Height);

            ///
            HalfHeight = Height / 2.0f;
            HalfWidth = Width / 2.0f;

            ///
            MinX = CenterPoint.x - HalfWidth;
            MaxX = CenterPoint.x + HalfWidth;
            MinY = CenterPoint.y - HalfHeight;
            MaxY = CenterPoint.y + HalfHeight;

            ///
            CalculatePlayfieldRect();

            ///
            CalculateTapMinFarDistance();

            ///
            IsCalculated = true;
            OnCalculated?.Invoke();
        }

        private void CalculateTapMinFarDistance()
        {
            ///
            TapMinFarDistanceSqr = HalfHeight * HalfHeight + HalfWidth * HalfWidth;

            ///
            TapMinFarDistance = Mathf.Sqrt(TapMinFarDistanceSqr);
        }

        private void CalculatePlayfieldRect()
        {
            var pos = new Vector2(MinX, MinY);
            var size = new Vector2(Width, Height);
            PlayfieldRect = new Rect(pos, size);
        }

        public Vector2 GetRandomPoint(Vector2 outwardOffset)
        {
            var effectiveMinX = MinX - outwardOffset.x;
            var effectiveMaxX = MaxX + outwardOffset.x;
            var effectiveMinY = MinY - outwardOffset.y;
            var effectiveMaxY = MaxY + outwardOffset.y;

            ///
            var x = Random.Range(effectiveMinX, effectiveMaxX);
            var y = Random.Range(effectiveMinY, effectiveMaxY);

            ///
            return new Vector2(x, y);
        }

        public Vector2 Clamp(Vector2 value)
        {
            return Clamp(value, Vector2.zero);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="padding">inward</param>
        /// <returns></returns>
        public Vector2 Clamp(Vector2 value, Vector2 padding)
        {
            ///
            return new Vector2()
            {
                x = Mathf.Clamp(value.x, -HalfWidth + padding.x, HalfWidth - padding.x),
                y = Mathf.Clamp(value.y, -HalfHeight + padding.y, HalfHeight - padding.y)
            };
        }

        public Vector3 Clamp(Vector3 value)
        {
            return Clamp(value, Vector2.zero);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="padding">inward</param>
        /// <returns></returns>
        public Vector3 Clamp(Vector3 value, Vector2 padding)
        {
            return new Vector3()
            {
                x = Mathf.Clamp(value.x, -HalfWidth + padding.x, HalfWidth - padding.x),
                y = Mathf.Clamp(value.y, -HalfHeight + padding.y, HalfHeight - padding.y),
                z = value.z
            };
        }

        public void DoAfterCalculated(Action action)
        {
            if (IsCalculated)
            {
                action?.Invoke();
            }
            else
            {
                OnCalculated += action;
            }
        }

        public Vector3 GetInwardVector(Vector3 startPoint)
        {
            return ((Vector3)CenterPoint - startPoint).normalized;
        }

        public Vector2 GetBlowVector(Vector2 startPoint)
        {
            var endPoint = Random.insideUnitCircle * blowRadius + CenterPoint;
            return (endPoint - startPoint).normalized;
        }

        public float GetDimensionValue(Dimension2D dimension)
        {
            switch (dimension)
            {
                case Dimension2D.Width:
                    return Width;
                case Dimension2D.Height:
                    return Height;
                default:
                    throw new NotImplementedException();
            }
        }

        public Vector2 GetRandomPointOnTheEdge()
        {
            return GetRandomPointOnTheEdge(out _, 0);
        }

        public Vector2 GetRandomPointOnTheEdge(float outwardOffset)
        {
            return GetRandomPointOnTheEdge(out _, outwardOffset);
        }

        public Vector2 GetRandomPointOnTheEdge(out PlayfieldEdge edge, float outwardOffset)
        {
            ///
            float x = 0;
            float y = 0;

            ///
            if (Random.value <= 0.5f) // Horizontal
            {
                ///
                x = Random.Range(-HalfWidth, HalfWidth);

                ///
                if (Random.value <= 0.5f) // Top
                {
                    ///
                    y = HalfHeight + outwardOffset;

                    ///
                    edge = PlayfieldEdge.Top;
                }
                else // Bottom
                {
                    ///
                    y = -HalfHeight - outwardOffset;

                    ///
                    edge = PlayfieldEdge.Bottom;
                }
            }
            else // Vertical
            {
                ///
                y = Random.Range(-HalfHeight, HalfHeight);

                ///
                if (Random.value <= 0.5f) // Left
                {
                    ///
                    x = -HalfWidth - outwardOffset;

                    ///
                    edge = PlayfieldEdge.Left;
                }
                else // Right
                {
                    ///
                    x = HalfWidth + outwardOffset;

                    ///
                    edge = PlayfieldEdge.Right;
                }
            }

            ///
            return CenterPoint + new Vector2(x, y);
        }

        public Vector2 GetRandomPointOnTheEdge(PlayfieldEdge edge, float outwardOffset)
        {
            ///
            float x = 0;
            float y = 0;

            ///
            if (edge == PlayfieldEdge.Top || edge == PlayfieldEdge.Bottom) // Horizontal
            {
                ///
                x = Random.Range(-HalfWidth, HalfWidth);

                ///
                if (edge == PlayfieldEdge.Top) // Top
                {
                    ///
                    y = HalfHeight + outwardOffset;
                }
                else // Bottom
                {
                    ///
                    y = -HalfHeight - outwardOffset;
                }
            }
            else // Vertical
            {
                ///
                y = Random.Range(-HalfHeight, HalfHeight);

                ///
                if (edge == PlayfieldEdge.Left) // Left
                {
                    ///
                    x = -HalfWidth - outwardOffset;
                }
                else // Right
                {
                    ///
                    x = HalfWidth + outwardOffset;
                }
            }

            ///
            return CenterPoint + new Vector2(x, y);
        }

        public Vector2 GetIntersection(Vector2 directionFromCenter, Vector2 outwardAmount)
        {
            ///
            float x = 0;
            float y = 0;
            float outwardX = 0;
            float outwardY = 0;

            ///
            if (Mathf.Approximately(0, directionFromCenter.x))
            {
                ///
                x = 0;

                ///
                if (directionFromCenter.y >= 0)
                {
                    y = HalfHeight;
                }
                else
                {
                    y = -HalfHeight;
                }

                ///
                outwardY = outwardAmount.y * Math.Sign(y);
            }
            else
            {
                // find k in the equation y = k*x
                float k = directionFromCenter.y / directionFromCenter.x;

                ///
                if (Mathf.Approximately(0, directionFromCenter.y)) // Horizontal line
                {
                    y = 0;
                    x = directionFromCenter.x > 0 ? HalfWidth : -HalfWidth;

                    ///
                    outwardX = outwardAmount.x * Math.Sign(x);
                }
                else if (directionFromCenter.y < 0) // bottom or right or left
                {
                    // check if bottom
                    x = -HalfHeight / k;
                    if ((x <= HalfWidth) && (x >= -HalfWidth))
                    {
                        ///
                        y = -HalfHeight;

                        ///
                        outwardY = outwardAmount.y * Math.Sign(y);
                    }
                    else
                    {
                        if (x > 0) // right
                        {
                            x = HalfWidth;
                            y = k * x;
                        }
                        else // left
                        {
                            x = -HalfWidth;
                            y = k * x;
                        }

                        ///
                        outwardX = outwardAmount.x * Math.Sign(x);
                    }
                }
                else // top or right or left
                {
                    // check if top
                    x = HalfHeight / k;
                    if ((x <= HalfWidth) && (x >= -HalfWidth))
                    {
                        ///
                        y = HalfHeight;

                        ///
                        outwardY = outwardAmount.y * Math.Sign(y);
                    }
                    else
                    {
                        if (x > 0) // right
                        {
                            x = HalfWidth;
                            y = k * x;
                        }
                        else // left
                        {
                            x = -HalfWidth;
                            y = k * x;
                        }

                        ///
                        outwardX = outwardAmount.x * Math.Sign(x);
                    }
                }
            }

            ///
            return new Vector2(x + outwardX, y + outwardY) + CenterPoint;
        }

        public bool IsInSameSideHorizontally(float x1, float x2)
        {
            return ((x1 - CenterPoint.x) * (x2 - CenterPoint.x)) > 0;
        }

        public bool IsInSameSideVertically(float y1, float y2)
        {
            return ((y1 - CenterPoint.y) * (y2 - CenterPoint.y)) > 0;
        }

        public float FlipSideHorizontally(float x)
        {
            return CenterPoint.x * 2.0f - x;
        }

        public float FlipSideVertically(float y)
        {
            return CenterPoint.y * 2.0f - y;
        }

        public PlayfieldEdge GetOppositeEdge(PlayfieldEdge playfieldEdge)
        {
            switch (playfieldEdge)
            {
                case PlayfieldEdge.Top:
                    return PlayfieldEdge.Bottom;
                case PlayfieldEdge.Bottom:
                    return PlayfieldEdge.Top;
                case PlayfieldEdge.Left:
                    return PlayfieldEdge.Right;
                case PlayfieldEdge.Right:
                    return PlayfieldEdge.Left;
                default:
                    throw new System.NotImplementedException();
            }
        }

        public PlayfieldEdge GetRandomPerpendicularEdge(PlayfieldEdge playfieldEdge)
        {
            if (playfieldEdge == PlayfieldEdge.Left || playfieldEdge == PlayfieldEdge.Right)
            {
                return Random.value <= 0.5f ? PlayfieldEdge.Top : PlayfieldEdge.Bottom;
            }
            else
            {
                return Random.value <= 0.5f ? PlayfieldEdge.Left : PlayfieldEdge.Right;
            }
        }

        public Vector2 GetEdgeInwardVector(PlayfieldEdge edge)
        {
            switch (edge)
            {
                case PlayfieldEdge.Top:
                    return Vector2.down;
                case PlayfieldEdge.Bottom:
                    return Vector2.up;
                case PlayfieldEdge.Left:
                    return Vector2.right;
                case PlayfieldEdge.Right:
                    return Vector2.left;
                default:
                    throw new System.NotImplementedException();
            }
        }

        public PlayfieldEdge GetRandomEdge()
        {
            return (PlayfieldEdge)Random.Range(0, 4);
        }

        public bool IsInside(Vector2 testPoint, Vector2 padding)
        {
            ///
            var rect = PlayfieldRect;
            rect.width -= padding.x * 2;
            rect.height -= padding.y * 2;

            ///
            return rect.Contains(testPoint);
        }

        public Vector3 FlipToFarDistance(Vector3 originalPos, Vector3 target)
        {
            ///
            Vector3 rs = originalPos;

            ///
            if (IsInSameSideHorizontally(originalPos.x, target.x))
            {
                rs.x = FlipSideHorizontally(originalPos.x);
            }

            ///
            if (IsInSameSideVertically(originalPos.y, target.y))
            {
                rs.y = FlipSideVertically(originalPos.y);
            }

            ///
            return rs;
        }

        /// <summary>
        /// imagine playfield is a texture, determine texcoord of a given world point
        /// </summary>
        /// <returns></returns>
        public Vector2 GetNormalizedTexcoord(Vector2 worldPosition)
        {
            return new Vector2()
            {
                x = Mathf.InverseLerp(MinX, MaxX, worldPosition.x),
                y = Mathf.InverseLerp(MinY, MaxY, worldPosition.y)
            };
        }

        /// <summary>
        /// imagine playfield is a texture, determine texcoord of a given world point
        /// </summary>
        /// <returns></returns>
        public Vector2 GetNormalizedTexcoordWithoutClamp(Vector2 worldPosition)
        {
            return new Vector2()
            {
                x = Mathg.InverseLerpWithoutClamp(MinX, MaxX, worldPosition.x),
                y = Mathg.InverseLerpWithoutClamp(MinY, MaxY, worldPosition.y)
            };
        }
    }
}
