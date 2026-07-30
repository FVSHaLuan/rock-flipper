using System.Collections.Generic;
using UnityEngine;

namespace Agame.Run.Combat
{
    public class PrototypeManager : ScriptableObjectWithInit
    {
        [SerializeField]
        private List<Rock> rocks = new List<Rock>();

        protected override void Init()
        {
            ///
            Validate();

            ///
            base.Init();
        }

#if UNITY_EDITOR
        private void Validate()
        {
            foreach (var rock in rocks)
            {
                ///
                if (rock == null)
                {
                    Debug.LogError($"Rock is null in PrototypeManager: {name}");
                }
                else
                {
                    var upperName = rock.name.ToUpper();
                    if (upperName == "ROCK" || upperName == "ROCK PURE")
                    {
                        Debug.LogError($"Base rocks are not allowed. Rock name: {rock.name}");
                    }
                }
            }
        }
#endif
    }

}