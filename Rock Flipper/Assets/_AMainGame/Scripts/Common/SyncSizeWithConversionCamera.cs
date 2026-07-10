using UnityEngine;

[RequireComponent(typeof(Camera))]
public class SyncSizeWithConversionCamera : ExtendedMonoBehaviour
{
    protected void OnEnable()
    {
        GetComponent<Camera>().orthographicSize = entry.conversionCamera.orthographicSize;
    }
}
