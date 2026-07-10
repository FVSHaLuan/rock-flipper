using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioThemeManager : ScriptableObject
{
    [SerializeField]
    private AudioThemeDefinition fallbackTheme;

    [Space]
    [SerializeField]
    private List<AudioThemeDefinition> themeDefinitions;

    public AudioThemeDefinition FallbackTheme => fallbackTheme;

    public int ThemeCount => themeDefinitions.Count;

    public AudioThemeDefinition GetTheme(int themeId)
    {
        return themeDefinitions[themeId];
    }
}
