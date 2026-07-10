
using System.Runtime.Serialization;

namespace Agame.Run
{
    [System.Serializable]
    public class CurrencyValueDictionary : SerializableDictionary<Currency, double>
    {
        public CurrencyValueDictionary() { }

        protected CurrencyValueDictionary(SerializationInfo information, StreamingContext context)
        {
        }
    }
}