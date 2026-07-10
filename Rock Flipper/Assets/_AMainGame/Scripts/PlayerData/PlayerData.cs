using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using FHC.Core.Architecture;
using UnityEngine.Rendering;
using System.Runtime.InteropServices.ComTypes;
using Agame.FeatureBranching;
using Agame.Balancing;

namespace Agame
{
    [Serializable]
    public partial class PlayerData
    {
        #region Cheating flags

        #endregion

        #region EVENTS

        #endregion EVENTS

        #region FIELDS
        [Header("0. Time log")]
        [SerializeField]
        private long lastTimeSaved = -99;
        [SerializeField]
        private long installTime = -1;
        [SerializeField]
        private float timeSpentInGame = 0;

        [Header("1. Version")]
        [SerializeField]
        private string lastLaunchVersion;
        [SerializeField]
        private string currentVersion;

        [Header("2. Player info")]
        [SerializeField]
        private int randomSeed;

        [Header("3. Common")]
        [SerializeField]
        private int lastSlotId = 0;
        [SerializeField]
        private MiniStorage generalStorage;

        [Header("4. Platform user")]
        [SerializeField]
        private string platformUserId;
        [SerializeField]
        private string pseudoUserId;
        #endregion

        #region NON-SERIALIZABLE FIELDS
        // common non-serializables
        [NonSerialized]
        private bool isCorrectingData;
        #endregion NON-SERIALIZABLE FIELDS

        #region PROPERTIES    

        #region 0. Time log - properties
        public long LastTimeSaved
        {
            get
            {
                return lastTimeSaved;
            }

            set
            {
                lastTimeSaved = value;
            }
        }

        /// <summary>
        /// by seconds
        /// </summary>
        public float TimeSpentInGame
        {
            get
            {
                return timeSpentInGame;
            }

            private set
            {
                timeSpentInGame = value;
            }
        }

        public long InstallTime
        {
            get
            {
                return installTime;
            }

            private set
            {
                installTime = value;
            }
        }
        #endregion 0. Time log - properties

        #region 1. Version - properties
        public Version LastLaunchVersion { get; private set; }
        public Version CurrentVersion { get; private set; }

        #endregion 1. Version - properties

        #region 2. Player info - properties
        public int RandomSeed => randomSeed;
        #endregion 2. Player info - properties

        #region 3. Common - properties
        public int LastSlotId
        {
            get => lastSlotId;
            set => lastSlotId = value;
        }
        public MiniStorage GeneralStorage
        {
            get
            {
                ///
                if (generalStorage == null)
                {
                    generalStorage = new MiniStorage();
                }

                ///
                return generalStorage;
            }
        }
        #endregion 3. Common - properties   

        #region 4. Platform user - properties
        public string PlatformUserId
        {
            get => platformUserId;
            set => platformUserId = value;
        }

        public string PseudoUserId => pseudoUserId;
        #endregion 4. Platform user - properties

        #region Un-grouped
        public bool UnlockedPremium => throw new System.NotImplementedException();
        #endregion Un-grouped
        #endregion PROPERTIES

        #region METHODS
        #region 0. Time log - methods 
        public void AddTimeSpentInGame(float seconds)
        {
            timeSpentInGame += seconds;
        }

        public bool TrySetNowAsInstallTime()
        {
            if (installTime < 0)
            {
                installTime = System.DateTime.Now.Ticks;
                return true;
            }

            ///
            return false;
        }
        #endregion 0. Time log - methods

        #region 1. Version - methods
        public void UpdateVersions()
        {
            ///
            lastLaunchVersion = currentVersion;
            currentVersion = Application.version;

            ///
            LastLaunchVersion = new Version(string.IsNullOrWhiteSpace(lastLaunchVersion) ? "0.0.0" : lastLaunchVersion);
            CurrentVersion = new Version(currentVersion);

            ///
            if (VersionBranchInfo.IsPlaytestOrDemo
                && CurrentVersion != LastLaunchVersion)
            {
                // runState = RunState.GameOvered;
            }
        }
        #endregion 1. Version - methods

        #region 4. Platform user - methods
        public void UpdatePseudoUserId()
        {
            ///
            if (!string.IsNullOrWhiteSpace(pseudoUserId))
            {
                return;
            }

            ///
            var h = new Hash128();

            ///
            h.Append((int)Application.platform);

            ///
            var deviceId = SystemInfo.deviceUniqueIdentifier;

            ///
            if (deviceId == SystemInfo.unsupportedIdentifier)
            {
                h.Append(System.DateTime.Now.Ticks);
                h.Append(UnityEngine.Random.value);
            }
            else
            {
                h.Append(deviceId);
            }

            ///
            pseudoUserId = h.ToString();
        }
        #endregion 4. Platform user - methods
        #endregion METHODS
    }

}