using UnityEngine;

namespace Agame.Dev
{
    /// <summary>
    /// Turns the target GameObject on/off according to <see cref="DevEntry.IsShowingDevUIs"/>.
    /// Subscribes in Awake/OnDestroy (not OnEnable/OnDisable) so it keeps listening while the target is off,
    /// which means this component's own GameObject must start active for Awake to run.
    /// Editor-only: <see cref="DevEntry.Instance"/> isn't available in players, so builds leave the target untouched.
    /// </summary>
    public class DevUIVisibility : MonoBehaviour
    {
        [SerializeField, Tooltip("Defaults to this GameObject when empty.")]
        private GameObject target;

        private GameObject Target => target != null ? target : gameObject;

#if UNITY_EDITOR
        protected void Awake()
        {
            ///
            DevEntry.OnDevUIsVisibilityChanged += Refresh;

            ///
            Refresh();
        }

        protected void OnDestroy()
        {
            DevEntry.OnDevUIsVisibilityChanged -= Refresh;
        }

        private void Refresh()
        {
            ///
            var devEntry = DevEntry.Instance;
            if (devEntry == null)
            {
                return;
            }

            ///
            Target.SetActive(devEntry.IsShowingDevUIs);
        }
#endif
    }
}
