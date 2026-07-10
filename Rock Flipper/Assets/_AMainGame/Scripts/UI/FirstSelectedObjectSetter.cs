using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Agame.UI
{
    public class FirstSelectedObjectSetter : ExtendedMonoBehaviour
    {
        [SerializeField]
        private Selectable initialSelectable;
        [SerializeField]
        private bool rememberLastSelected = true;

        private bool everSet = false;
        private Selectable lastSelected;

        protected void OnEnable()
        {
            ///
            SetFirstSelected();

            ///
            entry.uiSelectedEventManager.OnSelectionChanged += UiSelectedEventManager_OnSelectionChanged;
        }

        protected void OnDisable()
        {
            ///
            entry.uiSelectedEventManager.OnSelectionChanged -= UiSelectedEventManager_OnSelectionChanged;
        }

        private void UiSelectedEventManager_OnSelectionChanged()
        {
            ///
            var selected = entry.uiSelectedEventManager.LastSelectable;

            ///
            if (selected == null)
            {
                return;
            }

            ///
            if (selected.transform.IsChildOf(transform))
            {
                lastSelected = selected;
            }
        }

        private void SetFirstSelected()
        {
            if (!everSet || !rememberLastSelected)
            {
                ///
                entry.uiSelectedEventManager.SetCurrentSelectedGameObject(initialSelectable.gameObject);

                ///
                lastSelected = initialSelectable;
                everSet = true;
            }
            else
            {
                if (lastSelected != null)
                {
                    entry.uiSelectedEventManager.SetCurrentSelectedGameObject(lastSelected.gameObject);
                }
                else
                {
                    ///
                    entry.uiSelectedEventManager.SetCurrentSelectedGameObject(initialSelectable.gameObject);

                    ///
                    lastSelected = initialSelectable;
                }
            }
        }
    }

}