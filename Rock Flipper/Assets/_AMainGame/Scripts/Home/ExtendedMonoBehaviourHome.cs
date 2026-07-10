using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Agame.Home
{
    public class ExtendedMonoBehaviourHome : ExtendedMonoBehaviour
    {
        protected HomeEntry HomeEntry => HomeEntry.Instance;
    }

}