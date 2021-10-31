using System;
using UnityEngine;
using UnityEditor;

[Serializable]
public static class UILayout
{
    public static GUIStyle CustomizeGUIStyle(CustomLabel _Label)
    {
        GUIStyle style = new GUIStyle();

        style.normal.textColor = _Label.fontColor;
        style.normal.background = CreateLabelBackGround(_Label.cusTex2D.width, _Label.cusTex2D.height, _Label.cusTex2D.color);
        style.fontStyle = _Label.fontstyle;
        style.fontSize = _Label.fontSize;
        style.alignment = TextAnchor.MiddleCenter;

        return style;
    }

    public static Texture2D CreateLabelBackGround(int _Width, int _Height, Color _Color)
    {
        Texture2D texture2D = new Texture2D(_Width, _Height);

        Color32[] pixels = texture2D.GetPixels32();

        for (int i = 0; i < pixels.Length; ++i)
        {
            pixels[i] = _Color;
        }

        texture2D.SetPixels32(pixels);

        texture2D.Apply();
        return texture2D;
    }
}

[Serializable]
public struct CustomTex2D
{
    public int width;
    public int height;
    public Color color;

    public CustomTex2D(int _Width, int _Height, Color _Color)
    {
        this.width = _Width;
        this.height = _Height;
        this.color = _Color;
    }
};

[Serializable]
public struct CustomLabel
{
    public FontStyle fontstyle;
    public int fontSize;
    public Color fontColor;
    public CustomTex2D cusTex2D;

    public CustomLabel(FontStyle _FontStyle, int _FontSize, Color _FontColor, CustomTex2D _Tex2D)
    {
        this.fontstyle = _FontStyle;
        this.fontSize = _FontSize;
        this.fontColor = _FontColor;
        this.cusTex2D = _Tex2D;
    }
};