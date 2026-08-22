using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioRandomSeeker : MonoBehaviour
{
    [SerializeField, Range(0, 1)]
    private float min;
    [SerializeField, Range(0, 1)]
    private float max = 1;

    public void SeekRandomly()
    {
        ///
        var audioSource = GetComponent<AudioSource>();

        ///
        if (audioSource.clip == null)
        {
            return;
        }

        ///
        var clipLength = audioSource.clip.length;

        ///        
        var time = Random.Range(min, max) * clipLength;

        ///
        audioSource.time = time;
    }
}
