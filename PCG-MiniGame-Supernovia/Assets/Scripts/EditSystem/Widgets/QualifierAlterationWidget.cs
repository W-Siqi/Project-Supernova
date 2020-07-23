using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class QualifierAlterationWidget:Widget
{
    private QualiferAlteration editTarget;

    public QualifierAlterationWidget(QualiferAlteration editTarget) {
        this.editTarget = editTarget;
    }

    public override void RenderUI() {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(editTarget.targetQualifier.name);
        editTarget.type =(QualiferAlteration.Type) EditorGUILayout.EnumPopup(editTarget.type);
        EditorGUILayout.EndHorizontal();
    }
}
