using UnityEngine;

namespace Agame.Run
{
    public class SkillTreeScreen : UIScreen
    {
        public event System.Action OnClosed;

        protected override void OnDisable()
        {
            ///
            base.OnDisable();

            ///
            OnClosed?.Invoke();
        }
    }

}