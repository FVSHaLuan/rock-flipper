using System.Collections.Generic;
using UnityEngine;

namespace Agame.Run.Combat
{
    public class PrototypeManager : ScriptableObjectWithInit
    {
        [SerializeField]
        private List<Rock> rocks = new List<Rock>();
    }

}