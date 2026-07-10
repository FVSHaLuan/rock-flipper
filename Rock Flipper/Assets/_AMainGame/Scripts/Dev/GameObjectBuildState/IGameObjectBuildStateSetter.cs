using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Agame.Dev
{
    public interface IGameObjectBuildStateSetter
    {
        /// <summary>
        /// should be included in editor's GameObjectStateConfiguration
        /// </summary>
        bool IncludedInConfiguration { get; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns>true if set and no error</returns>
        bool SetBuildState();
    }
}