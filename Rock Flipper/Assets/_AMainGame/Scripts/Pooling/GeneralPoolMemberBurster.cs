using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralPoolMemberBurster : GeneralPoolMemberSpawner
{
    [Header("***GeneralPoolMemberBurster")]
    [SerializeField]
    private float radius = 0;
    [SerializeField]
    private int minCount;
    [SerializeField]
    private int maxCount;

    public override void Spawn()
    {
        ///
        var count = Random.Range(minCount, maxCount + 1);

        ///
        for (int i = 0; i < count; i++)
        {
            base.Spawn(); 
        }
    }

    protected override Vector3 GetEffectiveSpawningPosition()
    {
        return base.GetEffectiveSpawningPosition() + (Vector3)(Random.insideUnitCircle * radius);
    }
}
