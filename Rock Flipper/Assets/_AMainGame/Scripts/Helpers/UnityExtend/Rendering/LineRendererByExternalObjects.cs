using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
[ExecuteInEditMode]
public class LineRendererByExternalObjects : MonoBehaviour
{
    [SerializeField]
    private bool useLocalPosition = true;
    [SerializeField]
    private List<Transform> transforms;

    private LineRenderer lineRenderer;

    protected void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    protected void Update()
    {
        ///
        if (transforms == null || transforms.Count == 0)
        {
            ///
            lineRenderer.positionCount = 0;

            ///
            return;
        }

        ///
        lineRenderer.positionCount = transforms.Count;

        ///
        for (int i = 0; i < transforms.Count; i++)
        {
            var transform = transforms[i];
            var pos = useLocalPosition ? transform.localPosition : transform.position;
            lineRenderer.SetPosition(i, pos);
        }
    }
}
