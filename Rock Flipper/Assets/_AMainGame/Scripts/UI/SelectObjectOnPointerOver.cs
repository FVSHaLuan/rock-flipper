using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Selectable))]
public class SelectObjectOnPointerOver : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField]
    private UnityEvent onSelected;

    private Selectable selectable;

    protected void Awake()
    {
        selectable = GetComponent<Selectable>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ///
#if UNITY_ANDROID || UNITY_IOS
        return;
#endif

        ///
        if (!selectable.IsInteractable()
            || Entry.Instance.inputManager.ActiveSimplifiedInputDevice.deviceType != SimplifiedInputDeviceType.MouseAndKeyboard)
        {
            return;
        }

        ///
        EventSystem.current.SetSelectedGameObject(gameObject);

        ///
        onSelected?.Invoke();
    }
}
