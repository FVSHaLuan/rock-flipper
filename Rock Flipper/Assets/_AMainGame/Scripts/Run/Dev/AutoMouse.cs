using Agame.Run.Combat;
using UnityEngine;
using UnityEngine.UI;

namespace Agame.Run.Dev
{
    [RequireComponent(typeof(Toggle))]
    public class AutoMouse : ExtendedMonoBehaviourRun
    {
        [SerializeField]
        private PlayerCursor playerCursor;

        private Toggle toggle;
        private bool isLocking;

        protected void Start()
        {
            toggle = GetComponent<Toggle>();
            toggle.onValueChanged.AddListener(OnToggleValueChanged);
            OnToggleValueChanged(toggle.isOn);
        }

        protected void OnDestroy()
        {
            if (toggle != null)
            {
                toggle.onValueChanged.RemoveListener(OnToggleValueChanged);
            }
            SetLocking(false);
        }

        private void OnToggleValueChanged(bool isOn)
        {
            SetLocking(isOn);
        }

        private void SetLocking(bool locking)
        {
            if (isLocking == locking || playerCursor == null)
            {
                return;
            }

            ///
            isLocking = locking;
            if (locking)
            {
                playerCursor.AddFlippingLock(this);
            }
            else
            {
                playerCursor.RemoveFlippingLock(this);
            }
        }
    }
}
