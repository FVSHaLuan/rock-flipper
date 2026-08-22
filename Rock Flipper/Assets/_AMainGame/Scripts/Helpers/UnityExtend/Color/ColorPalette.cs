using OneLine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorPalette : ScriptableObjectWithInit
{
    [SerializeField]
    private Color testColor;

    [Space]
    [SerializeField]
    private ColorShades sampleColors = new ColorShades();

    [Space]
    [SerializeField]
    private List<ColorShades> colorShades = new List<ColorShades>();

    [System.Serializable]
    private class ColorShades
    {
        [SerializeField, OneLine]
        private List<Color> colors = new List<Color>();

        public Color FirstColor
        {
            get
            {
                ///
                if (colors == null || colors.Count == 0)
                {
                    return Color.white;
                }

                ///
                return colors[0];
            }
        }

        public void SetColors(Color hueSample, ColorShades otherShades)
        {
            ///
            if (colors == null)
            {
                colors = new List<Color>();
            }
            else
            {
                colors.Clear();
            }

            ///
            float hue;
            Color.RGBToHSV(hueSample, out hue, out _, out _);

            ///
            for (int i = 0; i < otherShades.colors.Count; i++)
            {
                ///
                float s;
                float v;
                Color.RGBToHSV(otherShades.colors[i], out _, out s, out v);

                ///
                var color = Color.HSVToRGB(hue, s, v);

                ///
                colors.Add(color);
            }
        }

        public int FindColor(Color testColor, out float distance)
        {
            ///
            if (colors == null || colors.Count == 0)
            {
                ///
                distance = -1;

                ///
                return -1;
            }

            ///
            int index = 0;
            var sqrdistance = colors[0].SqrHSVDistance(testColor);

            ///
            for (int i = 1; i < colors.Count; i++)
            {
                ///
                var newSqrDistance = colors[i].SqrHSVDistance(testColor);

                ///
                if (newSqrDistance < sqrdistance)
                {
                    sqrdistance = newSqrDistance;
                    index = i;
                }
            }

            ///
            distance = Mathf.Sqrt(sqrdistance);
            return index;
        }

        public Color GetColor(int shadeId)
        {
            return colors[shadeId];
        }
    }

#if UNITY_EDITOR
    [ContextMenu("Editor_Update")]
    private void Editor_Update()
    {
        ///
        UnityEditor.Undo.RegisterCompleteObjectUndo(this, "Update colors");

        ///
        foreach (var item in colorShades)
        {
            item.SetColors(item.FirstColor, sampleColors);
        }

        ///
        UnityEditor.EditorUtility.SetDirty(this);
    }

    [ContextMenu("Editor_FindTestColor")]
    private void Editor_FindTestColor()
    {
        ///
        int hueId = -1;
        int shadeId = -1;
        float distance = -1;

        ///
        for (int i = 0; i < colorShades.Count; i++)
        {
            ///
            var newShadeId = colorShades[i].FindColor(testColor, out float newDistance);

            ///
            if (hueId < 0 || newDistance < distance)
            {
                hueId = i;
                shadeId = newShadeId;
                distance = newDistance;
            }
        }

        ///
        var foundColor = Color.clear;
        if (shadeId >= 0)
        {
            foundColor = colorShades[hueId].GetColor(shadeId);
        }

        ///
        Debug.LogFormat("<color=#{3}>█</color><color=#{4}>█</color>HueId = {0}, shadeId = {1}, distance = {2}", hueId, shadeId, distance, ColorUtility.ToHtmlStringRGB(testColor), ColorUtility.ToHtmlStringRGB(foundColor));

        ///
        testColor = Color.white;

        ///
        UnityEditor.EditorUtility.SetDirty(this);
    }
#endif
}
