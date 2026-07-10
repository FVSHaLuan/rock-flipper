using System.Collections.Generic;
using UnityEngine;

namespace Agame.GamePlatform
{
    public static class DLCManager
    {
        public static event System.Action OnDLCAvailabilityChanged;

        public static DLCValidator DLCRockSkin = new SteamDLCValidator(DLCEnum.RockSkin, 3456800U, false);
        public static DLCValidator DLCAnimalSkin = new SteamDLCValidator(DLCEnum.AnimalSkin, 4865320U, true);
        public static DLCValidator DLCFoodSkin = new SteamDLCValidator(DLCEnum.FoodSkin, 4865330U, true);
        public static DLCValidator DLCFantasySkin = new SteamDLCValidator(DLCEnum.FantasySkin, 4865340U, true);
        public static DLCValidator DLCSpaceSkin = new SteamDLCValidator(DLCEnum.SpaceSkin, 4865350U, true);

        private static List<DLCValidator> validators = new List<DLCValidator>()
        {
            DLCRockSkin,
            DLCAnimalSkin,
            DLCFoodSkin,
            DLCFantasySkin,
            DLCSpaceSkin,
        };

        private static Dictionary<DLCEnum, DLCValidator> validatorDict;

        static DLCManager()
        {
            validatorDict = new Dictionary<DLCEnum, DLCValidator>();
            foreach (var validator in validators)
            {
                validatorDict.Add(validator.DLC, validator);
            }
        }

        public static void UpdateAllDlcAvailability()
        {
            ///
            foreach (var validator in validators)
            {
                validator.UpdateAvailability();
            }

            ///
            OnDLCAvailabilityChanged?.Invoke();
        }

        public static DLCValidator GetValidator(DLCEnum dLCEnum)
        {
            return validatorDict[dLCEnum];
        }

        public static bool IsAvailable(DLCEnum dlc)
        {
            return GetValidator(dlc).IsAvailable;
        }
    }
}