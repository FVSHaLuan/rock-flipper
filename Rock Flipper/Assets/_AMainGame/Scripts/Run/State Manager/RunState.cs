using UnityEngine;

namespace BT.Run
{
    public enum RunState
    {
        Init = -1,
        Combat = 0,
        BeforePrestige = 1,
        OnPrestige = 2,
        BeforeCombatFromPrestige = 3,
    }

}