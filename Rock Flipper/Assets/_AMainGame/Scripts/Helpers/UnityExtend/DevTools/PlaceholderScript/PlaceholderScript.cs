#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class PlaceholderScript : MonoBehaviour
{
#if UNITY_EDITOR
    [SerializeField]
    private MonoScript scriptReference;

    protected virtual bool ValidateScriptReference(MonoScript scriptReference)
    {
        return true;
    }

    [ContextMenu("Add Component")]
    private void AddComponent()
    {
        if (scriptReference == null)
        {
            Debug.LogWarning("Script reference is null.");
            return;
        }

        ///
        if (!scriptReference.GetClass().IsSubclassOf(typeof(Component)))
        {
            Debug.LogWarning("The referenced script is not a subclass of Component.");
        }

        ///
        if (!ValidateScriptReference(scriptReference))
        {
            Debug.LogWarning("The referenced script does not pass validation.");
            return;
        }

        ///
        var c = UnityEditor.Undo.AddComponent(this.gameObject, scriptReference.GetClass());

        ///
        Component[] allComponents = gameObject.GetComponents<Component>();
        int gap = -1;
        for (int i = 0; i < allComponents.Length; i++)
        {
            var component = allComponents[i];
            if (component == this)
            {
                gap = 0;
            }
            else if (component == c)
            {
                break;
            }
            else if (gap >= 0)
            {
                gap++;
            }
        }

        ///
        for (int i = 0; i < gap; i++)
        {
            UnityEditorInternal.ComponentUtility.MoveComponentUp(c);
        }

        ///
        UnityEditor.Undo.DestroyObjectImmediate(this);
    }

    protected void OnValidate()
    {
        ///
        if (scriptReference == null)
        {
            return;
        }

        if (!scriptReference.GetClass().IsSubclassOf(typeof(Component)))
        {
            Debug.LogWarning("The referenced script is not a subclass of Component.");
            scriptReference = null;
        }

        if (!ValidateScriptReference(scriptReference))
        {
            Debug.LogWarning("The referenced script does not pass validation.");
            scriptReference = null;
        }
    }
#endif
}
