using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnifiedText))]
public class UnifiedTextFromTextAsset : MonoBehaviour
{
    [SerializeField]
    private TextAsset textAsset;

    protected void Start()
    {
        GetComponent<UnifiedText>().Text = textAsset.text;
    }
}
