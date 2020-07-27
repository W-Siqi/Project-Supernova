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
        editTarget.delta.finance = EditorGUILayout.IntSlider(editTarget.delta.finance, -100, 100);
        GUILayout.Label("军队", GUILayout.Width(60));
        editTarget.delta.army = EditorGUILayout.IntSlider(editTarget.delta.army, -100, 100);
        GUILayout.Label("运气", GUILayout.Width(60));
        editTarget.delta.luck = EditorGUILayout.IntSlider(editTarget.delta.luck, -100, 100);
        EditorGUILayout.EndHorizontal();
    }
}
