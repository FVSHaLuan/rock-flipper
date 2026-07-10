using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleSystemPeriodicalBurster : MonoBehaviour
{
    [SerializeField]
    private bool playOnAwake = true;
    [SerializeField]
    private float interval = 5;
    [SerializeField]
    private float playDelay = 0;

    [Space]
    [SerializeField]
    private UnityEvent onBeforeBursting;
    [SerializeField]
    private UnityEvent onBursted;

    protected void OnEnable()
    {
        if (playOnAwake)
        {
            Play();
        }
    }

    [ContextMenu("Play")]
    public void Play()
    {
        StopAllCoroutines();
        StartCoroutine(PlayLoop(playDelay));
    }

    [ContextMenu("Stop")]
    public void Stop()
    {
        StopAllCoroutines();
    }

    private IEnumerator PlayLoop(float delay)
    {
        ///
        if (!Mathf.Approximately(delay, 0))
        {
            yield return new WaitForSeconds(delay);
        }

        ///
        var particleSystem = GetComponent<ParticleSystem>();
        var main = particleSystem.main;
        main.loop = false;

        ///
        while (true)
        {
            ///
            onBeforeBursting?.Invoke();

            ///            
            particleSystem.Stop(true);
            particleSystem.Play(true);

            ///
            onBursted?.Invoke();

            ///
            yield return new WaitForSeconds(interval);
        }
    }

#if UNITY_EDITOR
    protected void Reset()
    {
        var ps = GetComponent<ParticleSystem>();
        var main = ps.main;
        main.playOnAwake = false;
        main.loop = false;
        interval = main.duration;
    }
#endif
}
