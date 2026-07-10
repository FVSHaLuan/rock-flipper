using System.Runtime.Serialization;
using UnityEngine;

namespace Agame.Run
{
    [System.Serializable]
    public class CurrencyStateDictionary : SerializableDictionary<Currency, CurrencyState>
    {
        public CurrencyStateDictionary() { }

        protected CurrencyStateDictionary(SerializationInfo information, StreamingContext context)
        {
        }
    }

}