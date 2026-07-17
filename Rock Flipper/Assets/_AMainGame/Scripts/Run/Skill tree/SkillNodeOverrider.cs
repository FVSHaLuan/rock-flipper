using UnityEngine;

namespace Agame.Run
{
    [DisallowMultipleComponent]
    public abstract class SkillNodeOverrider : ExtendedMonoBehaviourRun
    {
        public event System.Action OnStateChanged;

        public SkillNode SkillNode { get; set; }

        protected void InvokeStateChanged()
        {
            OnStateChanged?.Invoke();
        }

        public virtual bool GetIsMaxable(out bool isMaxable)
        {
            isMaxable = true;
            return false;
        }
        public virtual bool GetLevelCount(out int levelCount)
        {
            levelCount = 0;
            return false;
        }
        public virtual bool HandleClick() => false;
        public virtual string GetCostString() => null;
        public virtual bool IsEnoughCurrencyToLevelUp(out bool result)
        {
            result = false;
            return false;
        }
        public virtual void OnActivate(bool applyToBuildStats, bool resetState) { }
        public virtual void OnDeactivate() { }
    }

}