using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class AudioChannelHelper : ExtendedMonoBehaviour
{
    [SerializeField]
    private GameAudioChannel gameAudioChannel = GameAudioChannel.Effect;

    public void ClearMute()
    {
        entry.audioManager.GetAudioChannel(gameAudioChannel).ClearMute();
    }

    public void ClearDim()
    {
        entry.audioManager.GetAudioChannel(gameAudioChannel).ClearDim();
    }

    public void AddMute()
    {
        entry.audioManager.GetAudioChannel(gameAudioChannel).AddMute(this);
    }

    public void RemoveMute()
    {
        entry.audioManager.GetAudioChannel(gameAudioChannel).RemoveMute(this);
    }

    public void AddDim()
    {
        entry.audioManager.GetAudioChannel(gameAudioChannel).AddDim(this);
    }

    public void RemoveDim()
    {
        entry.audioManager.GetAudioChannel(gameAudioChannel).RemoveDim(this);
    }

    [ContextMenu("AddMuteForLowerSubFxChannels")]
    public void AddMuteForLowerSubFxChannels()
    {
        ManipulateLowerSubFxChannels(channel => channel.AddMute(this));
    }

    [ContextMenu("RemoveMuteForLowerSubFxChannels")]
    public void RemoveMuteForLowerSubFxChannels()
    {
        ManipulateLowerSubFxChannels(channel => channel.RemoveMute(this));
    }

    [ContextMenu("AddDimForLowerSubFxChannels")]
    public void AddDimForLowerSubFxChannels()
    {
        ManipulateLowerSubFxChannels(channel => channel.AddDim(this));
    }

    [ContextMenu("RemoveDimForLowerSubFxChannels")]
    public void RemoveDimForLowerSubFxChannels()
    {
        ManipulateLowerSubFxChannels(channel => channel.RemoveDim(this));
    }

    private void ManipulateLowerSubFxChannels(System.Action<AudioChannelBase> callback)
    {
        ///
        var subChannels = entry.audioManager.EffectSubChannelsOrder;

        ///
        for (int i = 0; i < subChannels.Count; i++)
        {
            ///
            var currentGameAudioChannel = subChannels[i];
            if (currentGameAudioChannel == gameAudioChannel)
            {
                break;
            }

            ///
            var channel = entry.audioManager.GetAudioChannel(currentGameAudioChannel);
            callback?.Invoke(channel);
        }
    }
}
