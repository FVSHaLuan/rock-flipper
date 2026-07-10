using UnityEngine;
using System.Collections;
using System;

namespace FH.Core.Gameplay.HelperComponent
{
    public class LinearMover : OutsiteTargetTransform
    {
        [SerializeField, UnityEngine.Serialization.FormerlySerializedAs("velocity")]
        private Vector3 initialVelocity;
        [SerializeField]
        private Vector3 acceleration;

        private Vector3 velocity;

        public new Vector3 TargetPosition
        {
            get => base.TargetPosition;
            set => base.TargetPosition = value;
        }

        public Vector3 Acceleration
        {
            get => acceleration;
            set => acceleration = value;
        }

        public Vector3 Velocity
        {
            get
            {
                return velocity;
            }

            set
            {
                velocity = value;
            }
        }

        protected void OnDisable()
        {
            velocity = initialVelocity;
        }

        private void Update()
        {
            velocity += acceleration * Time.deltaTime;
            TargetPosition += velocity * Time.deltaTime;
        }
    }

}