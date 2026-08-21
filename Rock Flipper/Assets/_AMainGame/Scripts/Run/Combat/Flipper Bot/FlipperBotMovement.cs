using UnityEngine;

namespace Agame.Run.Combat
{
    public class FlipperBotMovement : ExtendedMonoBehaviourRun
    {
        private float Speed => BuildStats.flipperBotMovementSpeed;

        private Vector2 currentTarget;
        private bool hasTarget;

        protected void Update()
        {
            ChaseTarget();
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