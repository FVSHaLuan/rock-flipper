using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioThemeDefinition : ScriptableObject
{
    [SerializeField]
    private string themeName;

    [SerializeField]
    private AudioClip loopClip;
    [SerializeField]
    private AudioClip winClip;
    [SerializeField]
    private AudioClip loseClip;

    public string ThemeName => themeName;
    public AudioClip LoopClip => loopClip;
    public AudioClip WinClip => winClip;
    public AudioClip LoseClip => loseClip;
}