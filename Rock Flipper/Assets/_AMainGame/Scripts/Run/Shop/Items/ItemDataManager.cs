using System.Collections.Generic;
using UnityEngine;

namespace Agame.Run.Shop
{
    public class ItemDataManager : ScriptableObjectWithInit
    {
        [SerializeField]
        private List<ItemData> itemDataList = new List<ItemData>();
    }

}