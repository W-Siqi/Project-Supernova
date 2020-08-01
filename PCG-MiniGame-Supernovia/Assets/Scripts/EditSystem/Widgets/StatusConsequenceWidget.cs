using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;

public class StatusConsequenceWidget : Widget
{
    private StatusConsequence editTarget;

    public StatusConsequenceWidget(StatusConsequence editTarget) {
        this.editTarget = editTarget;    
    }

    public override void RenderUI() {
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("财政", GUILayout.Width(60));
        editTarget.delta.money = EditorGUILayout.IntSlider(editTarget.delta.money, -100, 100);
        GUILayout.Label("威信", GUILayout.Width(60));
        editTarget.delta.authority = EditorGUILayout.IntSlider(editTarget.delta.authority, -100, 100);
        GUILayout.Label("食物", GUILayout.Width(60));
        editTarget.delta.food = EditorGUILayout.IntSlider(editTarget.delta.food, -100, 100);
        EditorGUILayout.EndHorizontal();
    }
}
