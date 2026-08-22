using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LayerMaskExtension
{
    public static bool ContainsLayers(this LayerMask layerMask, int layers)
    {
        return layerMask == (layerMask | (1 << layers));
    }
}
