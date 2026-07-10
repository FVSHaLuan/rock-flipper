using Agame.Run;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

namespace Agame
{
    public class RunDataManager : ScriptableObjectWithInit
    {
        [SerializeField]
        private RunDataObject runDataZeroObject;
        [SerializeField]
        private RunDataObject runDataOneObject;
        [SerializeField]
        private RunDataObject runDataTwoObject;
        [SerializeField]
        private RunDataObject runDataThreeObject;

        [System.NonSerialized]
        private int activeRunDataIndex = 0;
        [System.NonSerialized]
        private RunDataObject activeRunDataObject;

        public int ActiveRunDataIndex
        {
            get => activeRunDataIndex;
            set
            {
                ///
                TryInit();

                ///
                if (RunEntry.Instance != null)
                {
                    throw new System.InvalidOperationException("Currently in a run");
                }

                ///
                if (value < 0 || value >= RunDataCount)
                {
                    throw new System.IndexOutOfRangeException("Invalid index for RunDataObject");
                }

                ///
                activeRunDataIndex = value;
                ActiveRunDataObject = GetRunDataObject(value);
            }
        }
        public int RunDataCount => 4;
        public RunDataObject ActiveRunDataObject
        {
            get
            {
                TryInit();
                return activeRunDataObject == null ? runDataZeroObject : activeRunDataObject;
            }
            set => activeRunDataObject = value;
        }

        protected override void Init()
        {
            ///
            activeRunDataIndex = Entry.Instance.PlayerData.LastSlotId;
            activeRunDataObject = GetRunDataObject(activeRunDataIndex);

            ///
            base.Init();
        }

        public RunDataObject GetRunDataObject(int index)
        {
            switch (index)
            {
                case 0:
                    return runDataZeroObject;
                case 1:
                    return runDataOneObject;
                case 2:
                    return runDataTwoObject;
                case 3:
                    return runDataThreeObject;
                default:
                    Debug.LogError("Invalid index for RunDataObject");
                    return null;
            }
        }
    }
}