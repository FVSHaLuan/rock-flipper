using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConcurrentActivationManager : MonoBehaviour
{
    public const int MaxActivationEventCount = 5;

    [SerializeField]
    private bool isUnscaledTime = true;

    private Dictionary<int, KeyState> keyStateByGeneralPoolPrototypeId = new Dictionary<int, KeyState>();
    private Dictionary<int, KeyState> keyStateByUnityObjectInstanceId = new Dictionary<int, KeyState>();

    public interface IKeyState
    {
        int LastUpdatedFrame { get; }
        int ActivationCountThisFrame { get; }
        float LastActivationTime { get; }

        int GetActivationCountInLastSeconds(float seconds);
    }

    private class KeyState : IKeyState
    {
        public int lastUpdatedFrame;
        public int activationCountThisFrame = 0;
        public float lastActivationTime = -1;

        private bool isUnscaledTime;

        private List<float> timeStamps;

        public int LastUpdatedFrame => lastUpdatedFrame;
        public int ActivationCountThisFrame => activationCountThisFrame;
        public float LastActivationTime => lastActivationTime;

        public KeyState(bool isUnscaledTime)
        {
            lastUpdatedFrame = Time.frameCount;
            this.isUnscaledTime = isUnscaledTime;
        }

        public int GetActivationCountInLastSeconds(float seconds)
        {
            ///
            if (timeStamps == null)
            {
                return 0;
            }

            ///
            var currentTime = GetTime();

            ///
            int count = 0;

            ///
            for (int i = timeStamps.Count - 1; i >= 0; i--)
            {
                ///
                var timeElapsed = currentTime - timeStamps[i];

                ///
                if (timeElapsed <= seconds)
                {
                    count++;
                }
                else
                {
                    break;
                }
            }

            ///
            return count;
        }

        public void ResetForCurrentFrameIfNeeded()
        {
            ///
            if (lastActivationTime != Time.frameCount)
            {
                activationCountThisFrame = 0;
            }
        }

        public void UpdateWithNewActivation()
        {
            ///
            ResetForCurrentFrameIfNeeded();

            ///
            lastActivationTime = GetTime();
            activationCountThisFrame++;

            ///
            UpdateTimeStampsWithNewActivation(lastActivationTime);
        }

        private void UpdateTimeStampsWithNewActivation(float time)
        {
            ///
            if (timeStamps == null)
            {
                timeStamps = new List<float>();
            }

            ///
            if (timeStamps.Count >= MaxActivationEventCount)
            {
                timeStamps.RemoveAt(0);
            }

            ///
            timeStamps.Add(time);
        }

        private float GetTime()
        {
            return isUnscaledTime ? Time.realtimeSinceStartup : Time.time;
        }
    }

    public IKeyState GetKeyStateByGeneralPoolPrototypeId(int prototypeId)
    {
        return GetKeyState(keyStateByGeneralPoolPrototypeId, prototypeId);
    }

    public IKeyState GetKeyStateByUnityObjectInstanceId(int instanceId)
    {
        return GetKeyState(keyStateByUnityObjectInstanceId, instanceId);
    }

    public void UpdateWithNewActivationByGeneralPoolPrototypeId(int prototypeId)
    {
        UpdateWithNewActivation(keyStateByGeneralPoolPrototypeId, prototypeId);
    }

    public void UpdateWithNewActivationByUnityObjectInstanceId(int instanceId)
    {
        UpdateWithNewActivation(keyStateByUnityObjectInstanceId, instanceId);
    }

    private void UpdateWithNewActivation(Dictionary<int, KeyState> dictionary, int key)
    {
        var ks = GetKeyState(dictionary, key);
        ks.UpdateWithNewActivation();
    }

    private KeyState GetKeyState(Dictionary<int, KeyState> dictionary, int key)
    {
        ///
        KeyState ks;

        ///
        if (dictionary.TryGetValue(key, out ks))
        {
            ///
            if (ks.lastUpdatedFrame != Time.frameCount)
            {
                ks.ResetForCurrentFrameIfNeeded();
            }
        }
        else
        {
            ks = new KeyState(isUnscaledTime);
            dictionary[key] = ks;
        }

        ///
        return ks;
    }
}
