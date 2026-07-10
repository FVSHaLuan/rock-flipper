using FH.Core.Architecture.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualAttachOnHit : ExtendedMonoBehaviour
{
    [SerializeField]
    private GeneralPoolMemberSimplified attachmentGuest;
    [SerializeField]
    private int maxAttachmentCount = 1000;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        ///
        var host = collision.collider.GetComponentInParent<IAttachmentHost>();

        ///
        if (host != null)
        {
            AttachTo(host);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ///
        var host = collision.GetComponentInParent<IAttachmentHost>();

        ///
        if (host != null)
        {
            AttachTo(host);
        }

    }

    private void AttachTo(IAttachmentHost host)
    {
        ///
        int guestId = attachmentGuest.PrototypeId;

        ///
        if (host.GetGuestCount(guestId) >= maxAttachmentCount)
        {
            return;
        }

        ///
        var guest = generalPool.TakeInstance(attachmentGuest, this).GetComponent<AttachmentGuest>();

        ///
        guest.gameObject.SetActive(true);

        ///
        guest.AttachTo(host, guestId);
    }
}
