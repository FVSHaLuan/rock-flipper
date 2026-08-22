using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PositionProgressView : MonoBehaviour
{
    [SerializeField]
    private Vector3 startPosition;
    [SerializeField]
    private Vector3 endPosition;
    [SerializeField]
    private bool isLocal;

    private IProgressValueProvider progressValueProvider;

    protected void Awake()
    {
        progressValueProvider = GetComponentInParent<IProgressValueProvider>();
    }

    protected void Update()
    {
#if UNITY_EDITOR
        if (!UnityEditor.EditorApplication.isPlaying)
        {
            if (progressValueProvider == null)
            {
                progressValueProvider = GetComponentInParent<IProgressValueProvider>();
            }
        }
#endif

        ///
        if (progressValueProvider == null)
        {
            enabled = false;
            return;
        }

        ///
        var pos = Vector3.Lerp(startPosition, endPosition, progressValueProvider.Progress);

        ///
        if (isLocal)
        {
            transform.localPosition = pos;
        }
        else
        {
            transform.position = pos;
        }
    }
}
