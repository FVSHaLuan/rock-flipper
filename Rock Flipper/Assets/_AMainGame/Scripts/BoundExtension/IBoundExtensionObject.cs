using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Agame
{
    public interface IBoundExtensionObject
    {
        BoundExtension BoundExtension { get; }
        BoundAddUpOptions AddUpOptions { get; }
    }
}