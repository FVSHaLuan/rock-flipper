using GD;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RandomAction : MonoBehaviour
{
    [SerializeField]
    private List<WeigtedAction> weigtedActions;

    [System.Serializable]
    public struct WeigtedAction : IWeighted
    {
        public string actionName;
        public float weight;
        public UnityEvent actionDelegate;

        public float Weight => weight;
    }

    [ContextMenu("TakeRandomAction")]
    public void TakeRandomAction()
    {
        ///
        if (weigtedActions == null || weigtedActions.Count == 0)
        {
            return;
        }

        ///
        weigtedActions.PickOne(UnityRandom.Default).actionDelegate?.Invoke();
    }
}
