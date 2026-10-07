using FH.Core.Architecture.Pool;
using UnityEngine;

namespace Agame.Run.Combat
{
    public class FloatingTextManager : FloatingTextManagerBase
    {
        public void Spawn(GeneralPoolMemberSimplifiedEffect prototype, Vector2 position, string text = null)
        {
            var floatingText = RunEntry.Instance.GeneralPool.TakeInstance(prototype, this);
            floatingText.gameObject.SetActive(false);
            if (text != null)
            {
                floatingText.SetText(text);
            }
            floatingText.gameObject.transform.position = position;
            floatingText.gameObject.SetActive(true);
        }
    }

}