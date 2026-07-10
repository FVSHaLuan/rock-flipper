using UnityEngine;
using UnityEngine.Events;

namespace Agame.Run.Combat
{
    public class BoundaryCollisionEvents : SpecificCollisionEvents
    {
        protected override string TargetTag => GameConst.TagBoundary;
    }
}