using GD;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class StatisticalWeight : MonoBehaviour, IWeighted
{
    [SerializeField]
    private float weight = 1;

    public float Weight => weight;
}
