using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Collision2DExtension
{
    public static ContactPoint2D GetRandomContactPoint(this Collision2D collision)
    {
        var i = Random.Range(0, collision.contactCount);
        return collision.GetContact(i);
    }
}
