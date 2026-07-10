using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllEffectChannel : AudioChannelBase
{
    public override event Action OnEffectiveVolumeChanged
    {
        add { throw new System.NotImplementedException(); }
        remove { throw new System.NotImplementedException(); }
    }

    public override float EffectiveVolume => throw new InvalidOperationException("Can't read EffectiveVolume of AllEffectChannel. Use EffectChannle or SpecialEffectChannel instead");

    private AllEffectChannel() { }

    public override void ClearMute()
    {
        foreach (var item in Entry.Instance.audioManager.AllEffectSubChannels)
        {
            item.ClearMute();
        }
    }

    public override void ClearDim()
    {
        foreach (var item in Entry.Instance.audioManager.AllEffectSubChannels)
        {
            item.ClearDim();
        }
    }

    public override void AddDim(object @object)
    {
        foreach (var item in Entry.Instance.audioManager.AllEffectSubChannels)
        {
            item.AddDim(@object);
        }
    }

    public override void AddMute(object @object)
    {
        foreach (var item in Entry.Instance.audioManager.AllEffectSubChannels)
        {
            item.AddMute(@object);
        }
    }

    public override void RemoveDim(object @object)
    {
        foreach (var item in Entry.Instance.audioManager.AllEffectSubChannels)
        {
            item.RemoveDim(@object);
        }
    }

    public override void RemoveMute(object @object)
    {
        foreach (var item in Entry.Instance.audioManager.AllEffectSubChannels)
        {
            item.RemoveMute(@object);
        }
    }
}
