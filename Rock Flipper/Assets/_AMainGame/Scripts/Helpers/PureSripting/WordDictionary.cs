using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordDictionary : MonoBehaviour
{
    [SerializeField]
    private List<string> words = new List<string>();

    public string GetRandomWord()
    {
        ///
        if (words == null || words.Count == 0)
        {
            return null;
        }

        ///
        return words[Random.Range(0, words.Count)];
    }
}
