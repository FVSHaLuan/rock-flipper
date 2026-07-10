using System.Runtime.Serialization;
using UnityEngine;

namespace BT.Run
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