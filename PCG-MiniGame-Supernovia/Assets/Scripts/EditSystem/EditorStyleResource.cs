﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EditorStyleResource
{
    public const float SEARCH_BAR_WIDTH = 100f;
    public const float BUTTON_WIDTH = 30f;

    private static Texture2D defaultTex = new Texture2D(10,10);
    private const string PRECONDITION_IMAGE_PATH = "Assets/ArtResourse/UI/frame.png";
    private const string CONSEQUENCE_IMAGE_PATH = "Assets/ArtResourse/UI/red.png";

    /// <summary>
    /// 会返回默认tex，如果加载失败
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static Texture2D LoadTex(string path) { 
        var tex = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
        if (tex) {
            return tex;
        }
        return defaultTex;
    }

    public static GUIStyle preconditionBlockStyle{
        get {
            var s = new GUIStyle();
            s.normal.background = LoadTex(PRECONDITION_IMAGE_PATH);
            return s;
        }
    }

    public static GUIStyle consequenceBlockStyle {
        get {
            var s = new GUIStyle();
            s.normal.background = LoadTex(CONSEQUENCE_IMAGE_PATH);
            return s;
        }
    }
}
