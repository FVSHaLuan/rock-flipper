using FH.Core.Gameplay.HelperComponent;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GameAudioController))]
public class AudioPitchByRotatingScale : MonoBehaviour
{
    [SerializeField]
    private Rotator rotator;

    [Header("Pitch range")]
    [SerializeField, Range(0, 3)]
    private float minPitch = 0;
    [SerializeField, Range(0, 3)]
    private float maxPitch = 1;

    [Header("Speed Scale range")]
    [SerializeField, Range(0, 1)]
    private float minScale = 0;
    [SerializeField, Range(0, 1)]
    private float maxScale = 1;

    private GameAudioController gameAudioController;

    protected void Awake()
    {
        gameAudioController = GetComponent<GameAudioController>();
    }

    protected void Update()
    {
        ///
        var scale = rotator.SpeedScale;

        ///
        var pitch = Mathf.Lerp(minPitch, maxPitch, Mathf.InverseLerp(minScale, maxScale, scale));

        ///
        gameAudioController.ControlPitch = pitch;
    }
}
