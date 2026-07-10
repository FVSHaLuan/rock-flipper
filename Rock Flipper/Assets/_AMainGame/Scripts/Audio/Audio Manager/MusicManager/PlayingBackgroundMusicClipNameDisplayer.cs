using UnityEngine;

public class PlayingBackgroundMusicClipNameDisplayer : ValueDisplayerUnified<int>
{
    [SerializeField]
    private string format = "{0}";

    protected override string GetString(int value)
    {
        return string.Format(format, BackgroundMusicPlayer.MusicName);
    }

    protected override int GetCurrentValue()
    {
        return BackgroundMusicPlayer.MusicId;
    }
}
