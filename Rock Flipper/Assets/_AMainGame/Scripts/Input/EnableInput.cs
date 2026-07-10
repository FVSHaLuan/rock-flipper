using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableInput : ExtendedMonoBehaviour
{
    protected override void ExtendedAwake()
    {
        entry.inputManager.InputActionAsset.Enable();
    }
}
