using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CSSToUnityUI : MonoBehaviour
{
    [TextArea(1,50)]
    public string cSSArea;
    public bool preserveImageAspect = true;
    public bool anchorsToCorners = true;
    CSSObject _CSSObject;
    

    public void ConvertCSS(string cssString)
    {
        string paresedCss = CSSParser.ToJSon(cssString);
        Debug.Log("New jSon object:\n" + paresedCss);
        _CSSObject = JsonUtility.FromJson<CSSObject>(paresedCss);

        RectTransform rectTransform = GetComponent<RectTransform>();
        Image image = GetComponent<Image>();
        Text text = GetComponent<Text>();

        Transform originalParent = transform.parent;
        int positionInHierarchy = transform.GetSiblingIndex();
        Transform canvasRoot = GetComponentInParent<Canvas>().transform;
        transform.SetParent(canvasRoot, false);

        float left = rectTransform.anchoredPosition.x;
        float top = rectTransform.anchoredPosition.y;
        float width = rectTransform.sizeDelta.x;
        float height = rectTransform.sizeDelta.y;

        rectTransform.pivot = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = Vector2.up;
        rectTransform.anchorMin = Vector2.up;

        
        if (!string.IsNullOrEmpty(_CSSObject.width))
            width = GetLayoutPropety(_CSSObject.width);
        
        if (!string.IsNullOrEmpty(_CSSObject.height))
            height = GetLayoutPropety(_CSSObject.height);
        
        rectTransform.sizeDelta = new Vector2(width, height);

        if (!string.IsNullOrEmpty(_CSSObject.left))
            left = GetLayoutPropety(_CSSObject.left);
       
        if (!string.IsNullOrEmpty(_CSSObject.top))
            top = GetLayoutPropety(_CSSObject.top);
       
        rectTransform.anchoredPosition = new Vector2(left + (width/2), -top - (height / 2));

        transform.SetParent(originalParent,true);
        transform.SetSiblingIndex(positionInHierarchy);

        if (anchorsToCorners)
            AnchorsToCorners(rectTransform);

        if (image != null)
        {
            if(preserveImageAspect)
                image.preserveAspect = true;

            if (!string.IsNullOrEmpty(_CSSObject.background_color))
            {
                Color newColor = new Color();
                ColorUtility.TryParseHtmlString(_CSSObject.background_color, out newColor);
                image.color = newColor;
            }
            if (!string.IsNullOrEmpty(_CSSObject.opacity))
            {
                float newOpacity = float.Parse(_CSSObject.opacity);
                Color newColor = image.color;
                newColor.a = newOpacity;
                image.color = newColor;
            }
        }

        if(text != null)
        {
            if (!string.IsNullOrEmpty(_CSSObject.color))
            {
                Color newColor = new Color();
                ColorUtility.TryParseHtmlString(_CSSObject.color, out newColor);
                text.color = newColor;
            }
            if (!string.IsNullOrEmpty(_CSSObject.text_align))
            {
                switch (_CSSObject.text_align)
                {
                    case "left":
                        text.alignment = TextAnchor.MiddleLeft;
                        break;
                    case "center":
                        text.alignment = TextAnchor.MiddleCenter;
                        break;
                    case "right":
                        text.alignment = TextAnchor.MiddleRight;
                        break;
                    default:
                        text.alignment = TextAnchor.MiddleCenter;
                        break;
                }
            }
            string fontFamily = "";
            int fontSize = 0;
            string fontStyle = "";
            float lineSpacing = 0f;

            if (!string.IsNullOrEmpty(_CSSObject.font))
            {
                string[] fontParts = _CSSObject.font.Split(new char[] { '_', ' ' });
                Debug.Log(fontParts.Length);
                if (fontParts.Length == 3)
                {
                    fontStyle = fontParts[0];
                    fontFamily = fontParts[2];
                    string fontparam = fontParts[1].Replace("px", "");
                    string[] fontparams = fontparam.Split('/');
                    if (fontparams.Length == 2)
                    {
                        if (!string.IsNullOrEmpty(fontparams[0]))
                        {
                            fontSize = int.Parse(fontparams[0]);
                        }
                        if (!string.IsNullOrEmpty(fontparams[1]))
                        {
                            lineSpacing = float.Parse(fontparams[1]) / fontSize;
                        }
                    }
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(_CSSObject.font_family))
                {
                    fontFamily = _CSSObject.font_family;
                }
                if (!string.IsNullOrEmpty(_CSSObject.font_style))
                {
                    fontStyle = _CSSObject.font_style;
                }
                if (!string.IsNullOrEmpty(_CSSObject.font_size))
                {
                    _CSSObject.font_size = _CSSObject.font_size.Replace("px", "");
                    fontSize = int.Parse(_CSSObject.font_size);
                }
                if (!string.IsNullOrEmpty(_CSSObject.line_height))
                {
                    _CSSObject.line_height = _CSSObject.line_height.Replace("px", "");
                    lineSpacing = float.Parse(_CSSObject.line_height);
                }
            }
           
            if (!string.IsNullOrEmpty(fontFamily))
            {
                Font[] allFonts = Resources.FindObjectsOfTypeAll<Font>();
                foreach (Font font in allFonts)
                {
                    string[] fontParts = font.name.Split(new char[] { '-', ' ' });
                    if (fontParts.Length == 1)
                    {
                        if (fontParts[0].Equals(fontFamily, System.StringComparison.OrdinalIgnoreCase))
                        {
                            text.font = font;
                        }
                    }
                    else if (fontParts.Length == 2 && !string.IsNullOrEmpty(fontStyle))
                    {
                        if (fontParts[0].Equals(fontFamily, System.StringComparison.OrdinalIgnoreCase) &&
                        fontParts[1].Equals(fontStyle, System.StringComparison.OrdinalIgnoreCase))
                        {
                            text.font = font;
                        }
                    }
                }
            }
            if (fontSize > 0)
            {
                text.fontSize = fontSize;
                text.resizeTextForBestFit = true;
                text.resizeTextMaxSize = fontSize;
            }
            if (lineSpacing > 0f)
            {
                text.lineSpacing = lineSpacing;
            }



            if (!string.IsNullOrEmpty(_CSSObject.text_transform))
            {
                switch (_CSSObject.text_transform)
                {
                    case "uppercase":
                        text.text = text.text.ToUpper();
                        break;
                    case "lowercase":
                        text.text = text.text.ToLower();
                        break;
                }
            }
        }
            
    }

    float GetLayoutPropety(string prop)
    {
        return float.Parse(prop.Remove(prop.IndexOf("px")));
    }


    void AnchorsToCorners(RectTransform t)
    {
        RectTransform pt = t.parent as RectTransform;

        Vector2 newAnchorsMin = new Vector2(t.anchorMin.x + t.offsetMin.x / pt.rect.width,
                                                t.anchorMin.y + t.offsetMin.y / pt.rect.height);
        Vector2 newAnchorsMax = new Vector2(t.anchorMax.x + t.offsetMax.x / pt.rect.width,
                                            t.anchorMax.y + t.offsetMax.y / pt.rect.height);

        t.anchorMin = newAnchorsMin;
        t.anchorMax = newAnchorsMax;
        t.offsetMin = t.offsetMax = new Vector2(0, 0);
    }
}


