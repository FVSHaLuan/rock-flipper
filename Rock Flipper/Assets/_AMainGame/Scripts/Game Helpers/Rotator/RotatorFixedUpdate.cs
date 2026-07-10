using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FH.Core.Gameplay.HelperComponent
{
    public class RotatorFixedUpdate : Rotator
    {
        protected override void Update()
        {
            // Intentionally left blank          
        }

        protected void FixedUpdate()
        {
            ///
            float deltaTime;
            if (gameplayUnscaledTime)
            {
                deltaTime = Entry.Instance.timeScaleManager.GameplayFixedUnscaledDeltaTime;
            }
            else if (unscaledTime)
            {
                deltaTime = Time.fixedUnscaledDeltaTime;
            }
            else
            {
                deltaTime = Time.fixedDeltaTime;
            }

            ///
            UpdateWithDeltaTime(deltaTime);
        }
    }

}