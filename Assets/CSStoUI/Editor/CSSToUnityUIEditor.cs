using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CSSToUnityUI))]
public class CSSToUnityUIEditor : Editor
{
    string cssString;
    public override void OnInspectorGUI()
    {
        CSSToUnityUI myTarget = (CSSToUnityUI)target;
        DrawDefaultInspector();
        if (GUILayout.Button("Convert CSS to Unity UI"))
        {
            cssString = myTarget.cSSArea;
            if (!string.IsNullOrEmpty(cssString))
            {
                myTarget.ConvertCSS(cssString);
            }
        }
    }

}

