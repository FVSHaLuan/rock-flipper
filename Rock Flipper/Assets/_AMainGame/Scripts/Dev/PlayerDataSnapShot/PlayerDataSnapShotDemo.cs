using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Agame.Dev
{
    [CreateAssetMenu(fileName = "PlayerDataSnapShotDemo", menuName = "BSB/Dev/PlayerDataSnapshot/PlayerDataSnapShotDemo")]
    public class PlayerDataSnapShotDemo : PlayerDataSnapShotBase
    {
        protected override PlayerDataObject PlayerDataObject => DevEntry.Instance.playerDataObjectDemo;
    }
}