using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace BT.UISystems
{
    public static class UISelectedHelper
    {
        [MenuItem("CONTEXT/Component/BSB/Set As Selected GameObject")]
        private static void SetAsSelectedGameObject(MenuCommand menuCommand)
        {
            ///
            var go = (menuCommand.context as Component)?.gameObject;

            ///
            if (go == null)
            {
                return;
            }

            ///
            Entry.Instance.uiSelectedEventManager.SetCurrentSelectedGameObject(go);
        }
    }
}