using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
# if UNITY_EDITOR
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
        GUILayout.Label("民心", GUILayout.Width(60));
        editTarget.delta.people = EditorGUILayout.IntSlider(editTarget.delta.people, -100, 100);
        GUILayout.Label("军队", GUILayout.Width(60));
        editTarget.delta.army = EditorGUILayout.IntSlider(editTarget.delta.army, -100, 100);
        EditorGUILayout.EndHorizontal();
    }
}
#endif