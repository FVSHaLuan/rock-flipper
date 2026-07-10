using UnityEngine;

public class ObjectPingger : MonoBehaviour
{
    [SerializeField]
    private GameObject targetObject;

    [ContextMenu("Ping Object"), PlayModeOnly]
    public void Ping()
    {
#if UNITY_EDITOR
        UnityEditor.EditorGUIUtility.PingObject(targetObject == null ? gameObject : targetObject);
#endif
    }
}
