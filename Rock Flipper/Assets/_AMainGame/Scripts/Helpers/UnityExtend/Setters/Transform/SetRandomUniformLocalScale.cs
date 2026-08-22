using FMod;
using OneLine;
using UnityEngine;

public class SetRandomUniformLocalScale : MonoBehaviour
{
    [SerializeField]
    private Vector3 baseScale = Vector3.one;
    [SerializeField, OneLineWithHeader]
    private RandomFloat scaleRange = new RandomFloat(0.5f, 1.5f);
    [SerializeField]
    private bool setOnEnabled = false;

    protected void OnEnable()
    {
        if (setOnEnabled)
        {
            Set();
        }
    }

    [ContextMenu("Set Random Uniform Local Scale"), PlayModeOnly]
    public void Set()
    {
        transform.localScale = baseScale * scaleRange;
    }
}
