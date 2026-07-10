using SubjectNerd.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ExternalCredits", menuName = "BSB/SingleInstance/ExternalCredits")]
public class ExternalCredits : ScriptableObject
{
    [Reorderable]
    public List<AssetCreditInfo> assets;
}
