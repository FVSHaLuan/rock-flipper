using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomObjectActivation : MonoBehaviour
{
    [SerializeField]
    private bool activateOnEnable;

    protected virtual void OnEnable()
    {
        if (activateOnEnable)
        {
            ActivateRandomObject();
        }
    }

    [ContextMenu("ActivateRandomObject")]
    public virtual void ActivateRandomObject()
    {
        ///
        int objectCount = transform.childCount;

        ///
        int selectedObjectId = Random.Range(0, objectCount);

        ///
        for (int i = 0; i < objectCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(i == selectedObjectId);
        }
    }
}
