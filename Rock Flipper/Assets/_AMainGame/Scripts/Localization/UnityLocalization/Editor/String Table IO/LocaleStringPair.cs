using UnityEngine;

namespace Agame.Localization
{
    [System.Serializable]
    public class LocaleStringPair
    {        
        public string localeCode;
        public string value;
        public string comment;

        public LocaleStringPair(string localeCode, string value, string comment)
        {
            this.localeCode = localeCode;
            this.value = value;
            this.comment = comment;
        }
    }

}