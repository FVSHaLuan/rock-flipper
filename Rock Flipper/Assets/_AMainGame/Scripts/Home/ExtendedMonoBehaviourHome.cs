using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BT.Home
{
    public class ExtendedMonoBehaviourHome : ExtendedMonoBehaviour
    {
        protected HomeEntry HomeEntry => HomeEntry.Instance;
    }

}