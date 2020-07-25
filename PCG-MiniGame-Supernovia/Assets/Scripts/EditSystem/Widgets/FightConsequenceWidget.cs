using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class FightConsequenceWidget : Widget
{
    private static string[] bindingOptions = { "随机", "角色1", "角色2", "角色3" };

    private FightConsequence editTarget = null;

    public FightConsequenceWidget(FightConsequence editTarget) {
        this.editTarget = editTarget;
    }

    public override void RenderUI() {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("进攻发起者",GUILayout.Width(35));
        editTarget.attackerbindFlag = EditorGUILayout.Popup(editTarget.attackerbindFlag, bindingOptions, GUILayout.Width(100));
        EditorGUILayout.LabelField("防守者", GUILayout.Width(35));
        editTarget.defenderbindFlag = EditorGUILayout.Popup(editTarget.defenderbindFlag, bindingOptions, GUILayout.Width(100));
        EditorGUILayout.EndHorizontal();
    }
}
