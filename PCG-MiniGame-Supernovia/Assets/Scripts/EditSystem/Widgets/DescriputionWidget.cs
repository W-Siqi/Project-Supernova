using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DescriputionWidget : Widget
{
    int mask = 0;
    public override void RenderUI() {
        var tempMask = new string[]{"前置 - 人物","前置 - 环境","前置 - 事件","后果 - 人物","后果 - 环境" };
        EditorGUILayout.BeginHorizontal();
        mask = EditorGUILayout.MaskField(mask,tempMask);
        EditorGUILayout.EndHorizontal();
    }
}
