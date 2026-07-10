using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiSpriteRendererMaterialChanger : MonoBehaviour
{
    [SerializeField]
    private Material material;

    private static List<SpriteRenderer> spriteRenderers = new List<SpriteRenderer>();

    [ContextMenu("ChangeAllChildrenAndSelf"), PlayModeOnly]
    public void ChangeAllChildrenAndSelf()
    {
        ///
        GetComponentsInChildren(spriteRenderers);

        ///
        foreach (var item in spriteRenderers)
        {
            item.sharedMaterial = material;
        }
    }
}
