using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BT
{
    public interface IBoundExtensionObject
    {
        BoundExtension BoundExtension { get; }
        BoundAddUpOptions AddUpOptions { get; }
    }
}