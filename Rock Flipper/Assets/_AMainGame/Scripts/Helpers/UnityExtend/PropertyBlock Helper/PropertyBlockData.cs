using SubjectNerd.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropertyBlockData : MonoBehaviour
{
    [SerializeReference, Reorderable]
    private List<Property> properties = new List<Property>();

    #region Property definitions
    [System.Serializable]
    private abstract class Property
    {
        public string propertyName;

        public abstract void SetValueToBlock(MaterialPropertyBlock materialPropertyBlock);

    }

    [System.Serializable]
    private class TextureProperty : Property
    {
        public Texture texture;
        public Vector2 scale = Vector2.one;
        public Vector2 offset = Vector2.zero;

        public override void SetValueToBlock(MaterialPropertyBlock materialPropertyBlock)
        {
            ///
            materialPropertyBlock.SetTexture(propertyName, texture);

            // texture parameters
            Vector4 tp = new Vector4(scale.x, scale.y, offset.x, offset.y);
            materialPropertyBlock.SetVector(propertyName + "_ST", tp);
        }
    }

    [System.Serializable]
    private class VectorProperty : Property
    {
        public Vector4 vector;

        public override void SetValueToBlock(MaterialPropertyBlock materialPropertyBlock)
        {
            materialPropertyBlock.SetVector(propertyName, vector);
        }
    }

    [System.Serializable]
    private class FloatProperty : Property
    {
        public float floatValue;

        public override void SetValueToBlock(MaterialPropertyBlock materialPropertyBlock)
        {
            materialPropertyBlock.SetFloat(propertyName, floatValue);
        }
    }

    [System.Serializable]
    private class ColorProperty : Property
    {
        public Color color;

        public override void SetValueToBlock(MaterialPropertyBlock materialPropertyBlock)
        {
            materialPropertyBlock.SetColor(propertyName, color);
        }
    }
    #endregion

    protected void Awake()
    {
        Debug.LogError("Shouldn't use this in runtime");
    }

    public void SetData(MaterialPropertyBlock materialPropertyBlock)
    {
        foreach (var item in properties)
        {
            item.SetValueToBlock(materialPropertyBlock);
        }
    }

#if UNITY_EDITOR
    [ContextMenu("Editor_AddTextureProperty")]
    private void Editor_AddTextureProperty()
    {
        properties.Add(new TextureProperty());
    }

    [ContextMenu("Editor_AddVectorProperty")]
    private void Editor_AddVectorProperty()
    {
        properties.Add(new VectorProperty());
    }

    [ContextMenu("Editor_AddFloatProperty")]
    private void Editor_AddFloatProperty()
    {
        properties.Add(new FloatProperty());
    }

    [ContextMenu("Editor_AddColorProperty")]
    private void Editor_AddColorProperty()
    {
        properties.Add(new ColorProperty());
    }
#endif
}
