using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MummyEffectManager : MonoBehaviour
{
    [SerializeField]
    private MummyEffectThread mummyThreadPrototype;
    [SerializeField]
    private Transform threadParent;

    [Space]
    [SerializeField, Min(1)]
    private int maxThreadCount = 10;
    [SerializeField, Range(0, 1)]
    private float effectProgress;

    private List<MummyEffectThread> activeMummyThreads = new List<MummyEffectThread>();

    public float EffectProgress { set => effectProgress = Mathf.Clamp01(value); }

    protected void Awake()
    {
        mummyThreadPrototype.gameObject.SetActive(false);
        Entry.Instance.GeneralPool.TryPushPrototype(mummyThreadPrototype);
    }

    protected void Update()
    {
        ///
        var targetThreadCount = (int)(maxThreadCount * effectProgress);
        if (targetThreadCount > activeMummyThreads.Count)
        {
            var addingCount = targetThreadCount - activeMummyThreads.Count;
            for (int i = 0; i < addingCount; i++)
            {
                AddSingleThread();
            }
        }
        else if (targetThreadCount < activeMummyThreads.Count)
        {
            RemoveThreads(activeMummyThreads.Count - targetThreadCount);
        }
    }

    private void RemoveThreads(int threadCount)
    {
        ///
        for (int i = 0; i < threadCount; i++)
        {
            activeMummyThreads[i].Disappear();
        }

        ///
        activeMummyThreads.RemoveRange(0, threadCount);
    }

    private void AddSingleThread()
    {
        var thread = Entry.Instance.GeneralPool.TakeInstance(mummyThreadPrototype, this) as MummyEffectThread;
        thread.transform.SetParent(threadParent);
        thread.gameObject.SetActive(true);
        activeMummyThreads.Add(thread);
    }
}
