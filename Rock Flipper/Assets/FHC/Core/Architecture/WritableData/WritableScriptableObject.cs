using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Assertions;

namespace FH.Core.Architecture.WritableData
{
    [ExecuteInEditMode]
    public class WritableScriptableObject<T> : ScriptableObject, IWritableData<T>, IWritableScriptableObjectHelper, IInspectorCommandObject where T : new()
    {
        [SerializeField, ReadOnly]
        private string key;
        [SerializeField, ReadOnly]
        private string standAloneKey;
        [SerializeField, ReadOnly]
        FileFormat fileFormat = FileFormat.JSON;
        [SerializeField, ReadOnly]
        private string password = "";
        [SerializeField, ReadOnly]
        private bool encryptOnStandalone = false;
        [SerializeField, ReadOnly]
        private bool encryptOnMobile = false;
        [SerializeField, ReadOnly]
        private bool encryptOnEditor = false;

        [Space]
        [SerializeField]
        private T editor_CurrentData;
        [SerializeField, InspectorCommand()]
        private int saveCurrentData;

        [Space]
        [SerializeField]
        protected T defaultData = new T();

        [NonSerialized]
        private bool loadedData = false;

        [NonSerialized]
        private T currentData;

        protected T CurrentData
        {
            get
            {
                return currentData;
            }

            set
            {
                currentData = value;
            }
        }

        public bool UseEncryption
        {
            get
            {
#if UNITY_STANDALONE

#if UNITY_EDITOR
                return encryptOnStandalone || encryptOnEditor;
#else
                return encryptOnStandalone;
#endif

#else

#if UNITY_EDITOR
                return encryptOnMobile || encryptOnEditor;
#else
                return encryptOnMobile;
#endif

#endif
            }
        }

        #region IWritableData<T>
        public T Data
        {
            get
            {
                if (!loadedData)
                {
                    LoadData();
                    loadedData = true;
                }

                return CurrentData;
            }

            set
            {
#if UNITY_EDITOR
                if (value == null)
                {
                    throw new ArgumentNullException();
                }
#endif
                CurrentData = value;
            }
        }
        public virtual void SaveData()
        {
            WritableDataManagerProvider.GetManager().SaveData(Key, CurrentData, fileFormat, UseEncryption, password);
        }
        #endregion

        protected T DefaultData
        {
            get
            {
                return defaultData;
            }
        }

        public string Key
        {
            get
            {
#if UNITY_IOS || UNITY_ANDROID
                return key;
#else
                return standAloneKey;
#endif
            }
        }

        protected void LoadData()
        {
            IWritableDataManager manager = WritableDataManagerProvider.GetManager();
            if (manager.ContainsKey(Key))
            {
                ///
                CurrentData = manager.LoadData<T>(Key, fileFormat, UseEncryption, password);

                ///
                if (CurrentData == null)
                {
                    Debug.LogErrorFormat("Had key for {0} but loaded null data!", key);
                }
            }

            ///
            if (CurrentData == null)
            {
                CurrentData = BinarySerializationHelper.Clone(defaultData);

            }

            ///
            OnDataLoaded(CurrentData);
        }

        string GetAutoKey()
        {
#if UNITY_EDITOR
            return UnityEditor.AssetDatabase.AssetPathToGUID(UnityEditor.AssetDatabase.GetAssetPath(this));
#else
            throw new System.NotImplementedException();
#endif
        }

        [ContextMenu("SetAutoKey")]
        protected void SetAutoKey()
        {
#if UNITY_EDITOR
            ///
            bool isDirty = false;

            ///
            var key = GetAutoKey();
            var standAloneKey = key + "_PC";

            ///
            isDirty = key != this.key || standAloneKey != this.standAloneKey;

            ///
            if (isDirty)
            {
                ///
                UnityEditor.Undo.RecordObject(this, "SetAutoKey");

                ///
                this.key = key;
                this.standAloneKey = standAloneKey;

                ///
                UnityEditor.EditorUtility.SetDirty(this);
            }
#endif
        }

        protected void CopyDefaultDataToCurrentData()
        {
            CurrentData = BinarySerializationHelper.Clone(defaultData);
        }

        public void CopyDefaultDataToCurrentData(WritableScriptableObject<T> dataObject)
        {
            CurrentData = BinarySerializationHelper.Clone(dataObject.defaultData);
        }

        protected virtual void OnDataLoaded(T data) { }
        #region MonoB

        public virtual void OnValidate()
        {
            ///
            if (string.IsNullOrWhiteSpace(key))
            {
                SetAutoKey();

                ///
                key = key.Trim();
                standAloneKey = standAloneKey.Trim();
            }

            ///
            Assert.IsFalse(string.IsNullOrEmpty(Key));
            Assert.IsFalse(string.IsNullOrEmpty(standAloneKey));
        }

        public void Reset()
        {
            SetAutoKey();
        }
        #endregion

        #region Context menu

        [ContextMenu("DisplayCurrentData")]
        public void Editor_DisplayCurrentData()
        {
            ///
            if (CurrentData == null)
            {
                LoadData();

                ///
                FHLog.Log("Loaded from " + Key);
            }

            ///
            editor_CurrentData = BinarySerializationHelper.Clone(CurrentData);
            FHLog.Log("Displayed current data");
        }

        [ContextMenu("SaveCurrentData")]
        protected void Editor_SaveCurrentData()
        {
            ///
            CurrentData = BinarySerializationHelper.Clone(editor_CurrentData);

            ///
            SaveData();

            ///
            FHLog.Log("Saved to " + Key);
        }

        [ContextMenu("CopyDefaultDataToCurrentData")]
        protected void Editor_CopyDefaultDataToCurrentData()
        {
            CopyDefaultDataToCurrentData();
        }

        #endregion

        #region IInspectorCommandObject
        void IInspectorCommandObject.ExcuteCommand(int intPara, string stringPara)
        {
            Editor_SaveCurrentData();
        }
        #endregion


    }

}