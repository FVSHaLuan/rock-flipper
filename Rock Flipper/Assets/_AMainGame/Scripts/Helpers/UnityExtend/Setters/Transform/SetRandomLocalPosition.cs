using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetRandomLocalPosition : MonoBehaviour
{
    [SerializeField]
    private float radius = 1;

    public void SetRandom()
    {
        transform.localPosition = Random.insideUnitCircle * radius;
    }

#if UNITY_EDITOR
    protected void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        var radiusScale = transform.parent == null ? 1 : Mathf.Max(transform.parent.lossyScale.x, transform.parent.lossyScale.y);
        Gizmos.DrawWireSphere(transform.position, radius * radiusScale);
    }
#endif
}
