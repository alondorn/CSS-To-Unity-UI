using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CSSParser : MonoBehaviour
{
    public static string ToJSon(string css)
    {
        string[] splittedCss = css.Split(new char[] { ':', ';' }, System.StringSplitOptions.RemoveEmptyEntries);
        string paresedCss = "{";
        bool isComma = false;
        for (int i = 0; i < splittedCss.Length; i++)
        {
            string prop1 = splittedCss[i].Trim();
            prop1 = '"' + prop1 + '"';
            prop1 = prop1.Replace('-', '_');
            prop1 = prop1.Replace(' ', '_');
            splittedCss[i] = prop1;

            paresedCss += splittedCss[i];
            if (isComma)
                paresedCss += ",";
            else
                paresedCss += ":";

            isComma = !isComma;
        }
        paresedCss = paresedCss.Remove(paresedCss.Length-1);
        paresedCss += "}";
       
        return paresedCss;
    }
}


[System.Serializable]
public class CSSObject
{
    public string top;
    public string left;
    public string width;
    public string height;
    public string background_color;

    public string text_align;
    public string font;
    public string letter_spacing;
    public string color;
    public string text_transform;
    public string opacity;
    public string font_family;
    public string font_size;
    public string font_style;
    public string line_height;
}
