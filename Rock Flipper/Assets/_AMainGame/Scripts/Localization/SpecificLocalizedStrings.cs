using UnityEngine;
using UnityEngine.Localization;

namespace Agame.Localization
{
    public class SpecificLocalizedStrings : ScriptableObjectWithInit
    {
        public static SpecificLocalizedStrings Instance
        {
            get
            {
                return Entry.Instance.specificLocalizedStrings;
            }
        }

  
    }

}