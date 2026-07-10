using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompleteInputBlockerHelper : MonoBehaviour
{
    [ContextMenu("AddBlockLock")]
    public void AddBlockLock()
    {
        Entry.Instance.completeInputBlocker.AddBlockLock(this);
    }

    [ContextMenu("RemoveBlockLock")]
    public void RemoveBlockLock()
    {
        Entry.Instance.completeInputBlocker.RemoveBlockLock(this);
    }
}
