using UnityEngine;
using System.Collections;

namespace FH.Core.Gameplay.HelperComponent
{
    public class Rotator : OutsiteTargetTransform
    {
        [SerializeField]
        private Vector3 angularSpeed = new Vector3();
        [SerializeField]
        private bool inversed = false;
        [SerializeField]
        private float speedScale = 1;

        [Space]
        [SerializeField]
        protected bool gameplayUnscaledTime = false;
        [SerializeField]
        protected bool unscaledTime = false;

        public Vector3 AngularSpeed
        {
            get
            {
                return angularSpeed;
            }

            set
            {
                angularSpeed = value;
            }
        }

        public float SpeedScale { get => speedScale; set => speedScale = value; }
        public bool Inversed { get => inversed; set => inversed = value; }

        protected virtual void Update()
        {
            ///
            float deltaTime;
            if (gameplayUnscaledTime)
            {
                deltaTime = Entry.Instance.timeScaleManager.GameplayUnscaledDeltaTime;
            }
            else if (unscaledTime)
            {
                deltaTime = Time.unscaledDeltaTime;
            }
            else
            {
                deltaTime = Time.deltaTime;
            }

            ///
            UpdateWithDeltaTime(deltaTime);
        }

        protected void UpdateWithDeltaTime(float deltaTime)
        {
            ///            
            Rotate(AngularSpeed * deltaTime * (Inversed ? -1 : 1) * SpeedScale);
        }

#if UNITY_EDITOR
        protected override void Reset()
        {
            ///
            base.Reset();

            ///
            if (GetComponent<RectTransform>() != null)
            {
                gameplayUnscaledTime = true;
                unscaledTime = true;
            }
        }
#endif
    }

}