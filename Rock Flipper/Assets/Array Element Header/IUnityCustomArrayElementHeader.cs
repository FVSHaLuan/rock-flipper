using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUnityCustomArrayElementHeader
{
    public string GetHeader(int index);

    public void UpdateHeader() { }
}
