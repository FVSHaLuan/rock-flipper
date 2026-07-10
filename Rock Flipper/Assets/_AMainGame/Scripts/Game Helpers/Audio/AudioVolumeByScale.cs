using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GameAudioController))]
public class AudioVolumeByScale : MonoBehaviour
{
    [Header("Volume range")]
    [SerializeField, Range(0, 1)]
    private float minVolume = 0;
    [SerializeField, Range(0, 1)]
    private float maxVolume = 1;

    [Header("Scale range")]
    [SerializeField, Range(0, 1)]
    private float minScale = 0;
    [SerializeField, Range(0, 1)]
    private float maxScale = 1;

    [Space]
    [SerializeField]
    private bool useLocalScale = false;

    private GameAudioController gameAudioController;

    protected void Awake()
    {
        gameAudioController = GetComponent<GameAudioController>();
    }

    protected void Update()
    {
        ///
        var scale = useLocalScale ? transform.localScale.x : transform.lossyScale.x;

        ///
        var volume = Mathf.Lerp(minVolume, maxVolume, Mathf.InverseLerp(minScale, maxScale, scale));

        ///
        gameAudioController.ControlVolume = volume;
    }
}
