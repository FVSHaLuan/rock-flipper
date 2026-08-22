using UnityEngine;

public class GeneralGizmosBaseLines : GeneralGizmos
{
#if UNITY_EDITOR
    [SerializeField]
    private float xLength = 1f;
    [SerializeField]
    private float yLength = 1f;

    protected override void DrawGizmosSpecific()
    {
        ///
        base.DrawGizmosSpecific();

        ///
        Gizmos.DrawLine(transform.position - new Vector3(xLength / 2, 0, 0), transform.position + new Vector3(xLength / 2, 0, 0));
        Gizmos.DrawLine(transform.position - new Vector3(0, yLength / 2, 0), transform.position + new Vector3(0, yLength / 2, 0));
    }
#endif
}
