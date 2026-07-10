using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class UnifiedText : MonoBehaviour
{
    [SerializeField]
    private Text uguiText;
    [SerializeField]
    private TMP_Text tmp_Text;

    public bool EnabledRenderer
    {
        get => MaskableGraphic.enabled;
        set => MaskableGraphic.enabled = value;
    }

    public string Text
    {
        get
        {
            if (uguiText != null)
            {
                return uguiText.text;
            }
            else if (tmp_Text != null)
            {
                return tmp_Text.text;
            }
            else
            {
                throw new System.Exception();
            }
        }

        set
        {
            SetText(value);
        }
    }

    public Color Color
    {
        get
        {
            if (uguiText != null)
            {
                return uguiText.color;
            }
            else if (tmp_Text != null)
            {
                return tmp_Text.color;
            }
            else
            {
                throw new System.Exception();
            }
        }

        set
        {
            if (uguiText != null)
            {
                uguiText.color = value;
            }
            else if (tmp_Text != null)
            {
                tmp_Text.color = value;
            }
            else
            {
                throw new System.Exception();
            }
        }
    }

    public int MaxVisibleCharacter
    {
        get
        {
            if (uguiText != null)
            {
                return -1;
            }
            else if (tmp_Text != null)
            {
                return tmp_Text.maxVisibleCharacters;
            }
            else
            {
                throw new System.Exception();
            }
        }

        set
        {
            if (uguiText != null)
            {
                Debug.LogWarning("uguiText Not supported MaxVisibleCharacter");
            }
            else if (tmp_Text != null)
            {
                tmp_Text.maxVisibleCharacters = value;
            }
            else
            {
                throw new System.Exception();
            }
        }
    }


    public FontStyle FontStyle
    {
        get
        {
            if (uguiText != null)
            {
                return uguiText.fontStyle;
            }
            else if (tmp_Text != null)
            {
                return GetFontStyle(tmp_Text.fontStyle);
            }
            else
            {
                throw new System.Exception();
            }
        }

        set
        {
            if (uguiText != null)
            {
                uguiText.fontStyle = value;
            }
            else if (tmp_Text != null)
            {
                tmp_Text.fontStyle = GetFontStyles(value);
            }
            else
            {
                throw new System.Exception();
            }
        }
    }

    public float Size
    {
        get
        {
            if (uguiText != null)
            {
                return uguiText.fontSize;
            }
            else if (tmp_Text != null)
            {
                return tmp_Text.fontSize;
            }
            else
            {
                throw new System.Exception();
            }
        }

        set
        {
            if (uguiText != null)
            {
                uguiText.fontSize = (int)value;
            }
            else if (tmp_Text != null)
            {
                tmp_Text.fontSize = value;
            }
            else
            {
                throw new System.Exception();
            }
        }

    }

    public float PreferredHeight
    {
        get
        {
            if (uguiText != null)
            {
                return uguiText.preferredHeight;
            }
            else if (tmp_Text != null)
            {
                return tmp_Text.preferredHeight;
            }
            else
            {
                throw new System.Exception();
            }
        }
    }

    public MaskableGraphic MaskableGraphic
    {
        get
        {
            if (uguiText != null)
            {
                return uguiText;
            }
            else if (tmp_Text != null)
            {
                return tmp_Text;
            }
            else
            {
                throw new System.Exception();
            }
        }
    }

#if UNITY_EDITOR
    public void Reset()
    {
        uguiText = GetComponent<Text>();
        tmp_Text = GetComponent<TMP_Text>();
    }

    [ContextMenu("SwitchToTMP")]
    public void SwitchToTMP()
    {
        ///
        UnityEditor.Undo.RecordObject(this.gameObject, "SwitchToTMP");

        ///
        if (tmp_Text != null)
        {
            uguiText = null;
            return;
        }

        string savedString = "";

        ///
        var toDestroyUGUIText = GetComponent<Text>();

        ///
        if (uguiText != null)
        {
            savedString = uguiText.text;
        }
        else if (toDestroyUGUIText != null)
        {
            savedString = toDestroyUGUIText.text;
        }

        ///
        if (toDestroyUGUIText != null)
        {
            DestroyImmediate(toDestroyUGUIText);
        }

        ///
        uguiText = null;

        ///
        tmp_Text = gameObject.AddComponent<TextMeshProUGUI>();
        tmp_Text.text = savedString;
    }

    [ContextMenu("SwitchToUGUIText")]
    public void SwitchToUGUIText()
    {
        ///
        UnityEditor.Undo.RecordObject(this.gameObject, "SwitchToUGUIText");

        ///
        if (uguiText != null)
        {
            tmp_Text = null;
            return;
        }

        ///
        string savedString = "";

        ///
        var toDestroyTmp_Text = GetComponent<TMP_Text>();

        ///
        if (tmp_Text != null)
        {
            savedString = tmp_Text.text;
        }
        else if (toDestroyTmp_Text != null)
        {
            savedString = toDestroyTmp_Text.text;
        }

        ///
        if (toDestroyTmp_Text != null)
        {
            DestroyImmediate(toDestroyTmp_Text);
        }

        ///
        tmp_Text = null;

        ///
        uguiText = gameObject.AddComponent<Text>();
        uguiText.text = savedString;
    }

#endif

    public void SetText(string text)
    {
        if (uguiText != null)
        {
            uguiText.text = text;
        }
        else if (tmp_Text != null)
        {
            tmp_Text.text = text;
        }
        else
        {
            throw new System.Exception();
        }
    }

    public FontStyle GetFontStyle(FontStyles fontStyles)
    {
        switch (fontStyles)
        {
            case FontStyles.Normal:
                return FontStyle.Normal;
            case FontStyles.Bold:
                return FontStyle.Bold;
            case FontStyles.Italic:
                return FontStyle.Italic;
            default:
                return FontStyle.Normal;
        }
    }

    public FontStyles GetFontStyles(FontStyle fontStyle)
    {
        switch (fontStyle)
        {
            case FontStyle.Normal:
                return FontStyles.Normal;
            case FontStyle.Bold:
                return FontStyles.Bold;
            case FontStyle.Italic:
                return FontStyles.Italic;
            default:
                return FontStyles.Normal;
        }
    }
}
