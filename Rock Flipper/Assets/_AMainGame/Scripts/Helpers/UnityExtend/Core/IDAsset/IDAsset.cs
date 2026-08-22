using UnityEngine;

public class IDAsset : ScriptableObject
{
    [SerializeField]
    private string id;

    public string ID => id;

#if UNITY_EDITOR
    protected void Reset()
    {
        id = System.Guid.NewGuid().ToString();
    }
#endif
}
