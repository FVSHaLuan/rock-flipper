using UnityEngine;

namespace Agame.GamePlatform
{
    public abstract class DLCValidator
    {
        private string prefKey;

        public string Id { get; private set; }
        public bool IsAvailable { get; private set; }
        public DLCEnum DLC { get; private set; }

        protected abstract bool GetCurrentAvailability();

        public virtual void InitiatePurchase() { }

        public DLCValidator(DLCEnum dlc)
        {
            Id = dlc.ToString();
            DLC = dlc;
            prefKey = "DLC_" + Id;

            ///
            IsAvailable = IsAvailableCached();
        }

        private bool IsAvailableCached()
        {
            return PlayerPrefs.GetInt(prefKey, 0) == 1;
        }

        public void UpdateAvailability()
        {
            var isAvailable = GetCurrentAvailability();
            if (isAvailable == IsAvailable)
            {
                return;
            }

            ///
            IsAvailable = isAvailable;

            ///
            PlayerPrefs.SetInt(prefKey, isAvailable ? 1 : 0);
        }
    }

}