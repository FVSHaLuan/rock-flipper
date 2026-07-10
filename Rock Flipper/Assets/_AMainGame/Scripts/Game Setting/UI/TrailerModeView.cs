using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Agame.UI.GameSettings
{
    public class TrailerModeView : ExtendedMonoBehaviour
    {
        [SerializeField]
        private UnityEvent onTrailerMode;

        protected void OnEnable()
        {
            if (gameSetting.trailerMode)
            {
                onTrailerMode?.Invoke();
            }
        }
    }

}