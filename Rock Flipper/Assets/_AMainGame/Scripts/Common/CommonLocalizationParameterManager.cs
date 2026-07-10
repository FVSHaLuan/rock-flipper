using I2.Loc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonLocalizationParameterManager : LocalizationParametersManager
{
    private Dictionary<string, ParameterValueFunction> parameterDictionary;

    public CommonLocalizationParameterManager()
    {
        parameterDictionary = new Dictionary<string, ParameterValueFunction>()
        {
            { "0", Zero},
            { "1", One},
            { "2", Two},
            { "3", Three},
            { "4", Four},
            { "5", Five},
        };
    }

    protected override Dictionary<string, ParameterValueFunction> ParameterDictionary => parameterDictionary;

    private static string Zero() => "{0}";
    private static string One() => "{1}";
    private static string Two() => "{2}";
    private static string Three() => "{3}";
    private static string Four() => "{4}";
    private static string Five() => "{5}";
}
