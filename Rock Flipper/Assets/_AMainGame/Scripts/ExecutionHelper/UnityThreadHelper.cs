using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FMod
{
    public class UnityThreadHelper : MonoBehaviour
    {
        private static UnityThreadHelper instance;
        public static UnityThreadHelper Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = (new GameObject()).AddComponent<UnityThreadHelper>();
                }

                ///
                return instance;
            }
        }

        List<System.Action> callbacks = new List<System.Action>();
        List<System.Action> callbackTmp = new List<System.Action>();

        private bool isExecutingCallbacks;

        public void Awake()
        {
            ///
            if (instance != null)
            {
                Destroy(gameObject);
                return;
            }

            ///
            DontDestroyOnLoad(gameObject);
        }

        public void DispatchToUnityThread(System.Action callback)
        {
            if (callback != null)
            {
                if (!isExecutingCallbacks)
                {
                    lock (this)
                    {
                        callbacks.Add(callback);
                    }
                }
                else
                {
                    callbackTmp.Add(callback);
                }
            }
        }

        public void LateUpdate()
        {
            ///
            isExecutingCallbacks = true;

            ///
            callbacks.AddRange(callbackTmp);
            callbackTmp.Clear();

            ///
            foreach (var item in callbacks)
            {
                item?.Invoke();
            }

            ///
            isExecutingCallbacks = false;

            ///
            callbacks.Clear();
        }
    }

}