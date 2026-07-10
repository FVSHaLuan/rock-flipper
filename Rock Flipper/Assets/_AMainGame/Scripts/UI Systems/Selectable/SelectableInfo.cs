using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class SelectableInfo : MonoBehaviour
{
    [SerializeField]
    private bool ignorePassiveSoundPlayer;
    [SerializeField]
    private bool neverSelectThis;

    public bool IgnorePassiveSoundPlayer => ignorePassiveSoundPlayer;
    public bool NeverSelectThis => neverSelectThis;
}
