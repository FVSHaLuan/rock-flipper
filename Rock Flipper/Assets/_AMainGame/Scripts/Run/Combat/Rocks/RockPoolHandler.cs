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

        [ContextMenu("Spawn As Old Rock"), PlayModeOnly]
        private void Editor_SpawnAsOldRock()
        {
            RunEntry.Instance.rockInstanceManager.SpawnAsOldRock(this);
        }

        [ContextMenu("Spawn As New Rock"), PlayModeOnly]
        private void Editor_SpawnAsNewRock()
        {
            RunEntry.Instance.rockInstanceManager.SpawnAsNewRock(this, new Vector2(5.5f, 0));
        }
#endif
    }

}