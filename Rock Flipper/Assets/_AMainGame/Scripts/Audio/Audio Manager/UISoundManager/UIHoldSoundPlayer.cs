using FH.Core.Architecture.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GameAudioController))]
public class UIHoldSoundPlayer : GeneralPoolMemberSimplified
{
    [SerializeField]
    private GameAudioPlayer activatedSound;

    public bool PlayActivatedSound { get; set; } = true;

    private AudioSource audioSource;

    public void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void StartHolding()
    {
        ///
        gameObject.SetActive(true);

        ///
        audioSource.Play();
    }

    public void Release(bool activated)
    {
        ///
        if (PlayActivatedSound && activated)
        {
            activatedSound.Play();
        }

        ///
        TryReturnToPoolAndDeactivate();
    }
}
