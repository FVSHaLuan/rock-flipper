using FH.Core.Architecture.Pool;
using UnityEngine;

namespace Agame.Run.Combat
{
    [RequireComponent(typeof(Rock))]
    public class RockPoolHandler : GeneralPoolMemberSimplified
    {
        [SerializeField, HideInNormalInspector]
        private Rock rock;

        public Rock Rock => rock;

#if UNITY_EDITOR
        protected void Reset()
        {
            rock = GetComponent<Rock>();
        }
#endif
    }

}