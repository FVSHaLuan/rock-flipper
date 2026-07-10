using FHC.Core.Architecture;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Agame
{
    public class PlayerDataSaver : ExtendedMonoBehaviour
    {
        public event System.Action OnBeforeSave;
        public event System.Action OnSaved;

        private float lastSavedTime;
        private BalancerWithObjects savableBalancer = new BalancerWithObjects();

        public bool SaveThisFrame { get; private set; } = false;

        public bool IsSavable => savableBalancer.IsBalanced;

        protected void Start()
        {
            ///
            if (entry.IsFirstLaunch)
            {
                entry.gameSettingObject.SaveData();
            }
        }

        public void SaveNow()
        {
            Save(entry.PlayerDataObject);
        }

        protected void LateUpdate()
        {
            ///
            if (SaveThisFrame)
            {
                ///
                Save(Entry.Instance.PlayerDataObject);

                ///
                SaveThisFrame = false;
            }
        }

        private bool Save(PlayerDataObject playerDataObject)
        {
            ///
            if (!savableBalancer.IsBalanced)
            {
                ///
                Debug.LogWarningFormat("Didn't save PlayerData because of active unsavableLock(s). Scene = {0}", Entry.ActiveGameScene);

                ///
                return false;
            }

            ///
            OnBeforeSave?.Invoke();

            ///
            playerDataObject.Data.AddTimeSpentInGame(Time.realtimeSinceStartup - lastSavedTime);
            lastSavedTime = Time.realtimeSinceStartup;

            ///
            playerDataObject.Data.LastTimeSaved = System.DateTime.Now.Ticks;
            playerDataObject.SaveData();

            ///            
            entry.runDataManager.ActiveRunDataObject?.SaveData();

            ///
            OnSaved?.Invoke();

            ///
            return true;
        }

        public void SetSaveThisFrame()
        {
            SaveThisFrame = true;
        }

        public void AddUnsavableLock(object @object)
        {
            ///
            savableBalancer.AddObject(@object);
        }

        public void RemoveUnsavableLock(object @object)
        {
            savableBalancer.RemoveObject(@object);
        }

        public void SaveAndAddUnsavableLock(object @object)
        {
            ///
            Save(entry.PlayerDataObject);

            ///
            AddUnsavableLock(@object);
        }

        protected void OnApplicationQuit()
        {
            entry.gameSettingObject.SaveData();
        }
    }

}