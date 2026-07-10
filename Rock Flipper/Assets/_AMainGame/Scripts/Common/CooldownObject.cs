using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CooldownObject
{
    private float cooldownTime;
    private float currentCooldownTime = -1;

    public float CooldownTime
    {
        get => cooldownTime;
        set
        {
            cooldownTime = value;
            currentCooldownTime = Mathf.Clamp(currentCooldownTime, 0, value);
        }
    }

    public bool IsCoolingDown => currentCooldownTime >= 0;

    public int RemainingTimeInSecond => (int)currentCooldownTime;

    /// <summary>
    /// 0 means it's done
    /// </summary>
    public float Progress => Mathf.Approximately(cooldownTime, 0) ? 1 : Mathf.Clamp01(currentCooldownTime / cooldownTime);

    public CooldownObject()
    {

    }

    public CooldownObject(float cooldownTime)
    {
        this.cooldownTime = cooldownTime;
    }

    public void StartCoolingDown()
    {
        currentCooldownTime = cooldownTime;
    }

    public void Update(float deltaTime)
    {
        ///
        if (!IsCoolingDown)
        {
            return;
        }

        ///
        currentCooldownTime -= deltaTime;
    }

    public void FinishCooldownImmediately()
    {
        currentCooldownTime = -1;
    }
}
