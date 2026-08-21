using UnityEngine;

namespace Agame.Run.Combat
{
    public class FlipperBotMovement : ExtendedMonoBehaviourRun
    {
        private float Speed => BuildStats.flipperBotMovementSpeed;

        private Vector2 GetNextTarget()
        {
            return Playfield.GetRandomPoint(-Vector2.one * 0.2f);
        }
    }

}