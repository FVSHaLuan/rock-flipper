using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class TextMeshColoredObject : UnifiedColoredObject
{
    private TMP_Text tmp_Text;

    protected override void SetColor(Color color)
    {
        if (tmp_Text == null)
        {
            tmp_Text = GetComponent<TMP_Text>();
        }

        tmp_Text.color = color;
    }
}
