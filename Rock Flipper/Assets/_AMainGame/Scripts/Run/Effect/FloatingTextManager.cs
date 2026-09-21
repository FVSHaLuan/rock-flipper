using FH.Core.Architecture.Pool;
using UnityEngine;

namespace Agame.Run.Combat
{
    public class FloatingTextManager : FloatingTextManagerBase
    {
        [SerializeField]
        private GeneralPoolMemberSimplifiedEffect prototype_0;

        public void Spawn(int prototypeId, Vector2 position, string text)
        {
            Spawn(prototype_0, position, text);
        }

        private void Spawn(GeneralPoolMemberSimplifiedEffect prototype, Vector2 position, string text)
        {
            var floatingText = RunEntry.Instance.GeneralPool.TakeInstance(prototype, this);
            floatingText.gameObject.SetActive(false);
            floatingText.SetText(text);
            floatingText.gameObject.transform.position = position;
            floatingText.gameObject.SetActive(true);
        }
    }

}