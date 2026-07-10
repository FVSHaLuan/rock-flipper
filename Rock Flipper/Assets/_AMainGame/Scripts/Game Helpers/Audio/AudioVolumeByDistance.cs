using FH.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GameAudioController))]
public class AudioVolumeByDistance : MonoBehaviour
{
    [Header("Volume range")]
    [SerializeField, Range(0, 1)]
    private float minVolume = 0;
    [SerializeField, Range(0, 1)]
    private float maxVolume = 1;

    [Header("Positions")]
    [SerializeField]
    private PositionProvider targetPositionProvider;
    [SerializeField, Min(0)]
    private float minDistance;
    [SerializeField, Min(0)]
    private float maxDistance;

    private GameAudioController gameAudioController;

    protected void Awake()
    {
        gameAudioController = GetComponent<GameAudioController>();
    }

    protected void Update()
    {
        ///
        var distance = Vector3.Distance(transform.position, targetPositionProvider.Position);

        ///
        var volume = Mathf.Lerp(maxVolume, minVolume, Mathf.InverseLerp(minDistance, maxDistance, distance));

        ///
        gameAudioController.ControlVolume = volume;
    }
}
