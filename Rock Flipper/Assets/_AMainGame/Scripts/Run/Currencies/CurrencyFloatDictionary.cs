using System.Runtime.Serialization;
using UnityEngine;

namespace Agame.Run
{
    [System.Serializable]
    public class CurrencyFloatDictionary : SerializableDictionary<Currency, float>
    {
        public CurrencyFloatDictionary() { }

        protected CurrencyFloatDictionary(SerializationInfo information, StreamingContext context)
        {
        }
    }

}