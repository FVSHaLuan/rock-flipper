using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BT
{
    public abstract class BoundExtensionObject : MonoBehaviour, IBoundExtensionObject
    {
        public abstract BoundExtension BoundExtension { get; }
        public abstract BoundAddUpOptions AddUpOptions { get; }
    }
}