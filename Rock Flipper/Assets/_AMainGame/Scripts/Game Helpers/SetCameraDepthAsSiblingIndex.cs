using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class SetCameraDepthAsSiblingIndex : MonoBehaviour
{
    [SerializeField]
    private float factor = 10;

#if UNITY_EDITOR
    protected void Awake()
    {
        SetDepth();
    }

    [ContextMenu("SetDepth")]
    private void SetDepth()
    {
        ///
#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            UnityEditor.Undo.RecordObject(GetComponent<Camera>(), "SetDepth");
        }
#endif

        ///
        var depth = transform.GetSiblingIndex() * factor;
        GetComponent<Camera>().depth = depth;

        ///
        GDebug.DoIfEnabledCheat(LogDepth);
    }

    private void LogDepth()
    {
        Debug.LogFormat("Cam {0} has depth = {1}", gameObject.name, GetComponent<Camera>().depth);
    } 
#endif
}
