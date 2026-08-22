using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class SetTextMeshProOutline : MonoBehaviour
{
    [SerializeField]
    private Color32 color = Color.black;
    [SerializeField]
    private float width = 0;

    [ContextMenu("UpdateOutline")]
    public void UpdateOutline()
    {
        var tmp = GetComponent<TMP_Text>();

        ///
        tmp.fontMaterial.SetFloat(ShaderUtilities.ID_OutlineWidth, width);
        tmp.fontMaterial.SetColor(ShaderUtilities.ID_OutlineColor, color);
    }

}
