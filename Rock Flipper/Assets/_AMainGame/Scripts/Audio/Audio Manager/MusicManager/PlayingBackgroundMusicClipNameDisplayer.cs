using Agame.Localization;
using UnityEngine;
using UnityEngine.Localization;

public class PlayingBackgroundMusicClipNameDisplayer : ValueDisplayerUnified<int>
{
    [SerializeField, Tooltip("{0}: music name, {1}: label")]
    private string format = "{0}";
    [SerializeField, Tooltip("Fills {1} in format, e.g. \"Now Playing\"")]
    private LocalizedString localizedLabel;

    private string label = string.Empty;

    protected override void OnEnable()
    {
        base.OnEnable();
        if (localizedLabel.IsValidAndNotEmpty())
        {
            localizedLabel.StringChanged += OnLabelChanged;
        }
    }

    private void OnDisable()
    {
        localizedLabel.StringChanged -= OnLabelChanged;
    }

    private void OnLabelChanged(string value)
    {
        label = value;
        ForceDisplay();
    }

    protected override string GetString(int value)
    {
        return string.Format(format, BackgroundMusicPlayer.MusicName, label);
    }

    protected override int GetCurrentValue()
    {
        return BackgroundMusicPlayer.MusicId;
    }
}
