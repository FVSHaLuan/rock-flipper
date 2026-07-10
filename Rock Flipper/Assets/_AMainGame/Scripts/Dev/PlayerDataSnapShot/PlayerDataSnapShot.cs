using FH.Core.Architecture.WritableData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BT.Dev
{
    [CreateAssetMenu(fileName = "PlayerDataSnapShot", menuName = "BSB/Dev/PlayerDataSnapshot/PlayerDataSnapShot")]
    public class PlayerDataSnapShot : PlayerDataSnapShotBase
    {
        protected override PlayerDataObject PlayerDataObject => DevEntry.Instance.playerDataObject;
    }
}