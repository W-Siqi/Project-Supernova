using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class QualifierWidget : Widget {
    private Qualifier editTarget;

    public QualifierWidget(Qualifier editTarget) {
        this.editTarget = editTarget;
    }

    public override void RenderUI() {
        EditorGUILayout.BeginHorizontal(EditorStyleResource.qualifierBlockStyle);
        EditorGUILayout.LabelField(editTarget.name, GUILayout.Width(25f));
        EditorGUILayout.EndHorizontal();
    }
}
