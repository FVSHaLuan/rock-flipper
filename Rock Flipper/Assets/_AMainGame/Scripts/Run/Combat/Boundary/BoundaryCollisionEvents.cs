using UnityEngine;
using UnityEngine.Events;

namespace BT.Run.Combat
{
    public class BoundaryCollisionEvents : SpecificCollisionEvents
    {
        protected override string TargetTag => GameConst.TagBoundary;
    }
}