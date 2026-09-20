using UnityEngine;

namespace Agame.Run.Combat
{
    public class FlipperBotMovement : ExtendedMonoBehaviourRun
    {
        [SerializeField]
        private float baseSpeed = 3f;

        private float Speed => baseSpeed * BuildStats.flipperBotMovementSpeedFactor;

        private FlipperBot flipperBot;
        private Vector2 currentTarget;
        private bool hasTarget;

        protected override void ExtendedAwake()
        {
            flipperBot = GetComponent<FlipperBot>();
        }

        protected void Update()
        {
            if (flipperBot.State == FlipperBot.FlipperBotState.Active)
            {
                ChaseTarget();
            }
        }

        private void ChaseTarget()
        {
            ///
            if (!hasTarget)
            {
                currentTarget = GetNextTarget();
                hasTarget = true;
            }

            ///
            transform.position = Vector2.MoveTowards(transform.position, currentTarget, Speed * Time.deltaTime);

            ///
            if ((Vector2)transform.position == currentTarget)
            {
                currentTarget = GetNextTarget();
            }
        }

        private Vector2 GetNextTarget()
        {
            return Playfield.GetRandomPoint(-Vector2.one * 0.2f);
        }
    }

}