using UnityEngine;

namespace Agame.Localization
{
    public static class LocalizedStringExtension
    {
        public static bool IsValidAndNotEmpty(this UnityEngine.Localization.LocalizedString localizedString)
        {
            if (localizedString == null)
            {
                return false;
            }

            ///
            if (localizedString.IsEmpty)
            {
                return false;
            }

            ///
            return true;
        }
    }

}