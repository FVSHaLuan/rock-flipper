using System.Collections.Generic;
using UnityEngine;

namespace Agame.Run.Shop
{
    public class ItemDataManager : ScriptableObjectWithInit
    {
        [SerializeField, UnityCustomArrayElementHeader]
        private List<ItemData> itemDataList = new List<ItemData>();
    }

}