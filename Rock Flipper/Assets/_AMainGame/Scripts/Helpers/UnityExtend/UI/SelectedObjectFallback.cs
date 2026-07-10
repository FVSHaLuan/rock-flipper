using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Selectable))]
public class SelectedObjectFallback : MonoBehaviour
{
    [SerializeField]
    private Selectable fallbackObject;
    [SerializeField]
    private List<Vector3> fallbackDirections;

    protected void OnDisable()
    {
        if (EventSystem.current.currentSelectedGameObject == gameObject)
        {
            Fallback();
        }
    }

    public void Fallback()
    {
        ///
        if (fallbackObject != null)
        {
            ///
            EventSystem.current.SetSelectedGameObject(fallbackObject.gameObject);

            ///
            return;
        }

        ///
        var selectable = GetComponent<Selectable>();

        ///
        for (int i = 0; i < fallbackDirections.Count; i++)
        {
            var direction = fallbackDirections[i];
            var found = selectable.FindSelectable(direction);
            if (found != null)
            {
                EventSystem.current.SetSelectedGameObject(found.gameObject);
                return;
            }
        }
    }
}
