using FHC.Core.Architecture;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBalancerWithObjectsBase<T> where T : class
{
    public event System.Action OnBalanceChanged;
    public event System.Action OnBalanced;
    public event System.Action OnOffBalanced;

    public static IBalancerWithObjectsBase<T> operator +(IBalancerWithObjectsBase<T> balancerWithObjects, T @object)
    {
        ///
        balancerWithObjects.AddObject(@object);

        ///
        return balancerWithObjects;
    }
    public static IBalancerWithObjectsBase<T> operator -(IBalancerWithObjectsBase<T> balancerWithObjects, T @object)
    {
        ///
        balancerWithObjects.RemoveObject(@object);

        ///
        return balancerWithObjects;
    }

    public bool IsBalanced { get; }

    public bool AddObject(T @object);
    public bool RemoveObject(T @object);

}
