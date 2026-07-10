using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Obsolete]
public class CursorVisibility : MonoBehaviour
{
    public bool IsVisible
    {
        set
        {
            Cursor.visible = value;
        }
    }
}
