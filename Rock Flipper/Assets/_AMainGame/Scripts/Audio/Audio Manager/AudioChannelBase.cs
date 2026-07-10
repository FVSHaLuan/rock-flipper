using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AudioChannelBase : MonoBehaviour
{
    public abstract event System.Action OnEffectiveVolumeChanged;

    public abstract float EffectiveVolume { get; }

    public abstract void AddMute(object @object);
    public abstract void RemoveMute(object @object);
    public abstract void AddDim(object @object);
    public abstract void RemoveDim(object @object);
    public abstract void ClearMute();
    public abstract void ClearDim();
}
