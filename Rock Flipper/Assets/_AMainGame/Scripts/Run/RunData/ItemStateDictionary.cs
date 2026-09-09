using Agame.Run;
using Agame.Run.Shop;
using System.Runtime.Serialization;
using UnityEngine;

namespace Agame
{
    [System.Serializable]
    public class ItemStateDictionary : SerializableDictionary<string, ItemState>
    {
        public ItemStateDictionary() { }

        protected ItemStateDictionary(SerializationInfo information, StreamingContext context)
        {
        }
    }

}