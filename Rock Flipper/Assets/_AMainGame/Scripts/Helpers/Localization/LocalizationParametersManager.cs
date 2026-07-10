using I2.Loc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LocalizationParametersManager : MonoBehaviourWithInit, ILocalizationParamsManager
{
    protected delegate string ParameterValueFunction();

    protected abstract Dictionary<string, ParameterValueFunction> ParameterDictionary { get; }

    protected void OnEnable()
    {
        LocalizationManager.ParamManagers.Add(this);
    }

    protected void OnDisable()
    {
        LocalizationManager.ParamManagers.Remove(this);
    }

    public string GetParameterValue(string Param)
    {
        return ParameterDictionary[Param]();
    }
}
