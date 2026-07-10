using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ScaleProgressView : MonoBehaviour
{
    [SerializeField]
    private Vector3 startScale;
    [SerializeField]
    private Vector3 endScale;

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
        var scale = Vector3.Lerp(startScale, endScale, progressValueProvider.Progress);

        ///
        transform.localScale = scale;
    }
}
