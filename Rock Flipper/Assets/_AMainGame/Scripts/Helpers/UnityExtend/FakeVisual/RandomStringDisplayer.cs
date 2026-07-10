using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomStringDisplayer : OneTimeTextDisplayer<string>
{
    [SerializeField]
    private string characters = "ABCDEFGHIKLMNOPQRSTVXYZabcdefghiklmnopqrstvxyz0123456789";
    [SerializeField]
    private int minLength = 1;
    [SerializeField]
    private int maxLength = 5;

    protected override string GetValue()
    {
        ///
        var length = Random.Range(minLength, maxLength + 1);

        ///
        return RandomString.GetRandomString(length, characters);
    }

    public void UpdateViewIfEnabled()
    {
        if (enabled)
        {
            UpdateView();
        }
    }
}
