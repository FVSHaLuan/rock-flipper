using FMod;
using UnityEngine;

[System.Serializable]
public struct RandomDouble
{
    [SerializeField]
    private double minValue;
    [SerializeField]
    private double maxValue;

    private double pinnedValue;

    public double PinnedValue
    {
        get
        {
            return pinnedValue;
        }

        private set
        {
            pinnedValue = value;
        }
    }

    public void PinAValue()
    {
        PinnedValue = this;
    }

    public double MinValue
    {
        get
        {
            return minValue;
        }

        private set
        {
            minValue = value;
        }
    }

    public double MaxValue
    {
        get
        {
            return maxValue;
        }

        private set
        {
            maxValue = value;
        }
    }

    public RandomDouble(double min, double max)
    {
        minValue = min;
        maxValue = max;
        pinnedValue = min;
    }

    public static implicit operator double(RandomDouble randomDouble)
    {   
        return Mathg.RandomInRange(randomDouble.MinValue, randomDouble.MaxValue);
    }
}
