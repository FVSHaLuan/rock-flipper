using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectWithName
{
    public string Name { get; private set; }

    public ObjectWithName(string name)
    {
        Name = name;
    }
}
