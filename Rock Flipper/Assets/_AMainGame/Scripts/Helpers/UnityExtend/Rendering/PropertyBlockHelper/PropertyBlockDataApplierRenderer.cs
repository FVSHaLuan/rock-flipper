using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer), typeof(PropertyBlockData))]
[ExecuteInEditMode]
public class PropertyBlockDataApplierRenderer : MonoBehaviour
{
    protected void Awake()
    {
        Apply();
    }

#if UNITY_EDITOR
    protected void Update()
    {
        if (!UnityEditor.EditorApplication.isPlaying)
        {
            Apply();
        }
    }
#endif

    [ContextMenu("Apply")]
    public void Apply()
    {
        ///
        var renderer = GetComponent<Renderer>();
        MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();
        renderer.GetPropertyBlock(propertyBlock);

        ///
        GetComponent<PropertyBlockData>().SetData(propertyBlock);

        ///
        renderer.SetPropertyBlock(propertyBlock);
    }
}
