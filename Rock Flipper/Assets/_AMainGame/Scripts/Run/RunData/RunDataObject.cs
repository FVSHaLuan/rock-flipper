using FH.Core.Architecture.WritableData;
using UnityEngine;

namespace BT
{
    public class RunDataObject : WritableScriptableObject<RunData>
    {
        protected override void OnDataLoaded(RunData data)
        {
            base.OnDataLoaded(data);

            ///
            data.CorrectData(defaultData);
        }

        public override void SaveData()
        {
            ///
            CurrentData.UpdateBeforeSave();

            ///
            base.SaveData();
        }

#if UNITY_EDITOR

#endif
    }

}