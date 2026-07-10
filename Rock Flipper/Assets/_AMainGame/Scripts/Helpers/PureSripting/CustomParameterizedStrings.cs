using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public static class CustomParameterizedStrings
{
    /// <summary>
    /// Parameters are in form [XXX]
    /// </summary>
    /// <param name="s"></param>
    /// <param name="parameterValueGetter"></param>
    /// <returns></returns>
    public static string ApplyParameters(this string s, MatchEvaluator parameterValueGetter)
    {
        ///
        if (string.IsNullOrWhiteSpace(s))
        {
            return s; 
        }

        ///
        return Regex.Replace(s, @"(?<=\[).*?(?=\])", parameterValueGetter).Replace("[", "").Replace("]", "");
    }
}
