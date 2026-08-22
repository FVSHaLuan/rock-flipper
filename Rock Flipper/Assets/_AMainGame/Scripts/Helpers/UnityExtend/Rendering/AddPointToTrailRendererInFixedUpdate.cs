using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TrailRenderer))]
public class AddPointToTrailRendererInFixedUpdate : MonoBehaviour
{
    private TrailRenderer trailRenderer;

    protected void Awake()
    {
        trailRenderer = GetComponent<TrailRenderer>();
    }

    protected void FixedUpdate()
    {
        trailRenderer.AddPosition(transform.position);
    }
}
