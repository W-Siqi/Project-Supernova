using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

// 生成动态的控件数组
// 支持删除
// TBD: list外部被clear掉的时候，wdgetDict对应的控件会仍然留在内存中的
public class DynamicWidgetGroup<WidgetT, WidgetEditTarget>where WidgetT: Widget{
    const string RENDER_UI_METHOD = "RenderUI";

    private Dictionary<WidgetEditTarget, WidgetT> widgetDict;
    private List<WidgetEditTarget> widgetEditTargets;
    private bool isHorizontal;

    public DynamicWidgetGroup(List<WidgetEditTarget> widgetEditTargets, bool isHorizontal = true) {
        this.isHorizontal = isHorizontal;
        widgetDict = new Dictionary<WidgetEditTarget, WidgetT>();
        this.widgetEditTargets = widgetEditTargets;
    }

    public void RenderUI() {
        if (isHorizontal) {
            EditorGUILayout.BeginHorizontal();
        }
        else {
            EditorGUILayout.BeginVertical();
        }

        List<WidgetEditTarget> toDelete = new List<WidgetEditTarget>();
        // 逐个绘制widget
        foreach (var target in widgetEditTargets) {
            if (!widgetDict.ContainsKey(target)) { 
                widgetDict[target] = (WidgetT)Activator.CreateInstance(typeof(WidgetT), target);
            }
            EditorGUILayout.BeginHorizontal();
            // render
            var w = widgetDict[target];
            w.GetType().GetMethod(RENDER_UI_METHOD).Invoke(w, null);
            // delete
            if (GUILayout.Button("X",GUILayout.Width(60),GUILayout.Width(60))) {
                toDelete.Add(target);
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();
        }

        // 删除对应widget
        foreach (var d in toDelete) {
            widgetDict.Remove(d);
            widgetEditTargets.Remove(d);
        }

        if (isHorizontal) {
            EditorGUILayout.EndHorizontal();
        }
        else {
            EditorGUILayout.EndVertical();
        }
    }
}
