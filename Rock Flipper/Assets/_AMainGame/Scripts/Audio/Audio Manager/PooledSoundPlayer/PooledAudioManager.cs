using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PooledAudioManager : ExtendedMonoBehaviour
{
    [SerializeField]
    private PooledGameAudioController pooledGameAudioControllerPrototype;

    private List<PooledGameAudioController> audioControllers = new List<PooledGameAudioController>();

    private int lastPlayId = 0;

    protected override void ExtendedAwake()
    {
        ///
        base.ExtendedAwake();

        ///
        pooledGameAudioControllerPrototype.gameObject.SetActive(false);
    }

    public void PutBackToPool(PooledGameAudioController pooledGameAudioController)
    {
        audioControllers.Add(pooledGameAudioController);
        pooledGameAudioController.IsInPool = true;
    }

    private PooledGameAudioController TakeAnAudioController()
    {
        ///
        PooledGameAudioController audioController = null;

        ///
        if (audioControllers.Count == 0)
        {
            audioController = Instantiate(pooledGameAudioControllerPrototype);
        }
        else
        {
            audioController = audioControllers[audioControllers.Count - 1];
            audioControllers.RemoveAt(audioControllers.Count - 1);
        }

        ///
        audioController.IsInPool = false;

        ///
        return audioController;
    }

    public PooledGameAudioController PlayAudio(PooledAudioPlayer pooledAudioPlayer)
    {
        ///
        var audioController = TakeAnAudioController();
        audioController.Play(pooledAudioPlayer, ++lastPlayId);

        ///
        return audioController;
    }
}
