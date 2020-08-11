//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEditor;

//public class QualifierWidget : Widget {
//    private Qualifier editTarget;

//    public QualifierWidget(Qualifier editTarget) {
//        this.editTarget = editTarget;
//    }

//    public override void RenderUI() {
//        EditorGUILayout.BeginHorizontal(EditorStyleResource.qualifierBlockStyle);
//        GUI.color = Color.red;
//        GUIStyle s = new GUIStyle();
//        EditorGUILayout.LabelField(editTarget.name, GUILayout.Width(25f), GUILayout.Height(25f));
//        GUI.color = Color.white;
//        EditorGUILayout.EndHorizontal();
//    }
//}
