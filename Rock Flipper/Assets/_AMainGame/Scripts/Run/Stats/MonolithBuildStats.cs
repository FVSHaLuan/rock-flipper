using UnityEngine;

namespace Agame.Run.Stats
{
    /// <summary>
    /// Stats shared by every monolith, one instance per <see cref="Agame.Run.Combat.MonolithType"/>.
    /// Monolith-specific stats (e.g. Prism's shooting interval) stay as their own fields on <see cref="BuildStatsObject"/>.
    /// </summary>
    [System.Serializable]
    public class MonolithBuildStats
    {
        public bool unlocked = false;
    }

}
