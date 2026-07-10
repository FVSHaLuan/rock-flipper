using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ContextButtonPrompt))]
public class MusicToggleButton : ExtendedMonoBehaviour
{
    [SerializeField]
    private string turnOffPrompt = "TURN MUSIC OFF";
    [SerializeField]
    private string turnOnPrompt = "TURN MUSIC ON";

    private ContextButtonPrompt contextButtonPrompt;

    protected override void ExtendedAwake()
    {
        ///
        base.ExtendedAwake();

        ///
        contextButtonPrompt = GetComponent<ContextButtonPrompt>();
    }

    protected void OnEnable()
    {
        UpdateView();
    }

    public void Toggle()
    {
        ///
        if (IsMusicOn())
        {
            entry.audioManager.MusicVolume = 0;
        }
        else
        {
            entry.audioManager.MusicVolume = entry.audioManager.DefaultMusicVolume;
        }

        ///
        UpdateView();
    }

    private void UpdateView()
    {
        if (IsMusicOn())
        {
            contextButtonPrompt.Text = turnOffPrompt;
        }
        else
        {
            contextButtonPrompt.Text = turnOnPrompt;
        }
    }

    private bool IsMusicOn()
    {
        return !Mathf.Approximately(entry.audioManager.MusicVolume, 0);
    }
}
