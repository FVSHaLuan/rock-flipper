using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AttachmentGuest : MonoBehaviourWithInit
{
    [SerializeField]
    private AttachmentSlot attachmentSlot;

    [Space]
    [SerializeField]
    private UnityEvent onShouldDetach;
    [SerializeField]
    private UnityEvent onAttached;
    [SerializeField]
    private UnityEvent onFailedToAttach;
    [SerializeField]
    private UnityEvent onDetached;

    private IAttachmentHost host;
    private int guestId;

    public void AttachTo(IAttachmentHost host, int guestId)
    {
        ///
        if (host == null)
        {
            onFailedToAttach?.Invoke();
        }

        ///
        var attachmentParent = host.GetAttachmentParent(attachmentSlot);
        if (attachmentParent == null)
        {
            onFailedToAttach?.Invoke();
        }

        ///
        this.host = host;
        this.guestId = guestId;

        ///
        host.OnShouldDetach += Host_OnShouldDetach;

        ///
        transform.SetParent(attachmentParent);

        ///
        host.IncreaseGuestCount(guestId);

        ///
        onAttached?.Invoke();
    }

    private void Host_OnShouldDetach()
    {
        ///
        onShouldDetach?.Invoke();

        ///
        Detach();
    }

    public void Detach()
    {
        ///
        if (host == null)
        {
            return;
        }

        ///
        host.OnShouldDetach -= Host_OnShouldDetach;

        ///
        transform.SetParent(null);

        ///
        host.DecreaseGuestCount(guestId);

        ///
        onDetached?.Invoke();

        ///
        host = null;
    }
}
