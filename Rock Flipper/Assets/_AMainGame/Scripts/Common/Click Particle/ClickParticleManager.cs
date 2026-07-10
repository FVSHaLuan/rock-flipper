using FH.Core.Architecture.Pool;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BT
{
    public class ClickParticleManager : ExtendedMonoBehaviour
    {
        [SerializeField]
        private GeneralPoolMemberSimplified clickParticlePrototype;

        public void Play()
        {
            ///
            if (Mouse.current == null)
            {
                return;
            }

            ///
            var clickParticle = generalPool.TakeInstance(clickParticlePrototype, this);

            ///
            var p = entry.GetPointerPositionViaConversionCamera();

            ///
            clickParticle.transform.position = p;
            clickParticle.gameObject.SetActive(true);
        }
    }

}