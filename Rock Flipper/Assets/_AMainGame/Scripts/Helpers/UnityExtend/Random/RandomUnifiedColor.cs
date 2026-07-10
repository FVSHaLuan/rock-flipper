using GD;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomUnifiedColor : MonoBehaviour
{
    [SerializeField]
    private bool setAtEnabled = true;

    [SerializeField]
    private List<UnifiedColoredObject> unifiedColoredObjects;

    [Space]
    [SerializeField]
    private List<Color> colors;

    protected void OnEnable()
    {
        if (setAtEnabled)
        {
            SetRandomColor();
        }
    }

    [ContextMenu("SetRandomColor")]
    public void SetRandomColor()
    {
        ///
        var color = colors.GetRandomItem();

        ///
        foreach (var item in unifiedColoredObjects)
        {
            item.Color = color;
        }
    }
}
