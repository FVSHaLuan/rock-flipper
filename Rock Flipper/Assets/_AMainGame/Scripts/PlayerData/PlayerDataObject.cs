using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FH.Core.Architecture.WritableData;

namespace Agame
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "BSB/SingleInstance/PlayerData")]
    public class PlayerDataObject : WritableScriptableObject<PlayerData>
    {
        protected override void OnDataLoaded(PlayerData data)
        {
            base.OnDataLoaded(data);

            ///
            data.CorrectData(defaultData);

            ///
            data.UpdateVersions();
            data.UpdatePseudoUserId();
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