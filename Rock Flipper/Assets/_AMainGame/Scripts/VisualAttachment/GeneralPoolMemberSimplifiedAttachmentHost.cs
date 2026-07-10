using FH.Core.Architecture.Pool;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralPoolMemberSimplifiedAttachmentHost : GeneralPoolMemberSimplified, IAttachmentHost
{
    private event Action OnShouldDetach;

    event Action IAttachmentHost.OnShouldDetach
    {
        add
        {
            OnShouldDetach += value;
        }

        remove
        {
            OnShouldDetach -= value;
        }
    }

    [Header("AttachmentHost")]
    [SerializeField]
    private Transform trailAttachmentParent;

    private Dictionary<int, int> guestCountById = new Dictionary<int, int>();

    public override bool TryReturnToPool()
    {
        ///
        if (!IsInPool)
        {
            OnShouldDetach?.Invoke();
        }

        ///
        var rs = base.TryReturnToPool();

        ///
        guestCountById.Clear();

        ///
        return rs;
    }

    public override bool TryReturnToPoolAndDeactivate()
    {
        ///
        if (!IsInPool)
        {
            ///
            OnShouldDetach?.Invoke();
        }

        ///
        guestCountById.Clear();

        ///
        return base.TryReturnToPoolAndDeactivate();
    }

    Transform IAttachmentHost.GetAttachmentParent(AttachmentSlot attachmentSlot)
    {
        switch (attachmentSlot)
        {
            case AttachmentSlot.Trail:
                return trailAttachmentParent;
            default:
                throw new System.NotImplementedException();
        }
    }

    int IAttachmentHost.GetGuestCount(int guestId)
    {
        ///
        int count;

        ///
        if (guestCountById.TryGetValue(guestId, out count))
        {
            return count;
        }
        else
        {
            return 0;
        }
    }

    void IAttachmentHost.IncreaseGuestCount(int guestId)
    {
        ///
        int count;

        ///
        if (guestCountById.TryGetValue(guestId, out count))
        {
            guestCountById[guestId] = count + 1;
        }
        else
        {
            guestCountById[guestId] = 1;
        }
    }

    void IAttachmentHost.DecreaseGuestCount(int guestId)
    {
        ///
        int count;

        ///
        if (guestCountById.TryGetValue(guestId, out count))
        {
            guestCountById[guestId] = count - 1;
        }
    }
}
