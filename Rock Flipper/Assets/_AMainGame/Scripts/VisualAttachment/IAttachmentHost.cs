using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttachmentHost
{
    event System.Action OnShouldDetach;
    
    Transform GetAttachmentParent(AttachmentSlot attachmentSlot);

    int GetGuestCount(int guestId);
    void IncreaseGuestCount(int guestId);
    void DecreaseGuestCount(int guestId);
}
