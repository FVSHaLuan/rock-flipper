using GD;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomWeightedObjectActivation : RandomObjectActivation
{
    private List<StatisticalWeight> weighteds = new List<StatisticalWeight>();

    public override void ActivateRandomObject()
    {
        ///
        weighteds.Clear();

        ///
        for (int i = 0; i < transform.childCount; i++)
        {
            ///
            var weighted = transform.GetChild(i).GetComponent<StatisticalWeight>();

            ///
            if (weighted != null)
            {
                weighteds.Add(weighted);
            }
        }

        ///
        int selectedId = -1;

        ///
        if (weighteds.Count > 0)
        {
            selectedId = weighteds.PickOne(UnityRandom.Default).transform.GetSiblingIndex();
        }

        ///
        for (int i = 0; i < transform.childCount; i++)
        {
            ///
            transform.GetChild(i).gameObject.SetActive(i == selectedId);
        }
    }
}
