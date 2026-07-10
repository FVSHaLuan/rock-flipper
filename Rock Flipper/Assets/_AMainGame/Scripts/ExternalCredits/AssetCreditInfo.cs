using SubjectNerd.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct AssetCreditInfo
{
    [Space]
    public Texture texture;
    public AudioClip audioClip;

    [TextArea]
    public string creditText;

    [Space]   
    [Reorderable]
    public List<string> creditUrls;
}
