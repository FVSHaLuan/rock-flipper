using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntryLoader : MonoBehaviour
{
    protected void Awake()
    {
        ///
        if (Entry.Instance != null)
        {
            return;
        }

        ///
        Instantiate(Resources.Load(GameConst.EntryResourcePath));
    }
}
