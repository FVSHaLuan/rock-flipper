using GD;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeightedFlexible : IWeighted
{
    bool IsFlexible { get; }
}
