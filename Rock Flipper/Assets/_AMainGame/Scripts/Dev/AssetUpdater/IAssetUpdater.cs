using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAssetUpdater
{
#if UNITY_EDITOR
    public void Editor_Update();
#else
    public void Editor_Update() {}
#endif
}
