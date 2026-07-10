using System.Collections.Generic;
using BT.Dev;
using UnityEngine;
using UnityEngine.Serialization;

namespace BT
{
    [CreateAssetMenu(menuName = "OV/SingleInstances/VisualDefinitions")]
    public class VisualDefinitions : ScriptableObjectWithInit
    {
        public Color normalTextColor;
        public Color notEnoughTextColor;

        public static VisualDefinitions Instance
        {
            get
            {
#if UNITY_EDITOR
                if (!Application.isPlaying)
                {
                    return DevEntry.Instance.visualDefinitions;
                }
#endif

                return Entry.Instance.visualDefinitions;
            }
        }
    }

}