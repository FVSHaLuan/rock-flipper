using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

namespace BT.Cores.HelperComponent
{
    [RequireComponent(typeof(Toggle))]
    [ExecuteInEditMode]
    public class ToggleObjectAttacher : MonoBehaviour
    {
        [SerializeField, HideInInspector]
        private Toggle toggle;

        [SerializeField]
        private List<GameObject> attachedObjects;
        [SerializeField]
        private List<GameObject> attachedObjectsOff;

        public void Awake()
        {
            if (toggle == null)
            {
                toggle = GetComponent<Toggle>();
            }
            SetAttachedObjectActive(toggle.isOn);
            toggle.onValueChanged.AddListener((bool isOn) => { SetAttachedObjectActive(isOn); });
        }

        private void SetAttachedObjectActive(bool active)
        {
            ///
            if (attachedObjects != null)
            {
                foreach (var item in attachedObjects)
                {
                    if (item != null)
                    {
                        item.SetActive(active);
                    }
                }
            }

            if (attachedObjectsOff != null)
            {
                foreach (var item in attachedObjectsOff)
                {
                    if (item != null)
                    {
                        item.SetActive(!active);
                    }
                }
            }
        }

#if UNITY_EDITOR
        public void Update()
        {
            if (!Application.isPlaying)
            {
                SetAttachedObjectActive(toggle.isOn);
            }
        }

        protected void Reset()
        {
            toggle = GetComponent<Toggle>();
        }
#endif
    }

}