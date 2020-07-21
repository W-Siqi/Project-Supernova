using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class QualifierEditField
{
    //private List<Qualifier> editTarget;
    //private QualifierLibrary attachedLibrary;
    private int selectIndex = 0;

    //public QualifierEditField(List<Qualifier> editTarget, QualifierLibrary attachedLibrary) {
    //    this.editTarget = editTarget;
    //    this.attachedLibrary = attachedLibrary;
    //}

    public void RenderUI(List<Qualifier> editTarget, QualifierLibrary attachedLibrary) {
        EditorGUILayout.LabelField("Test");
        NewQualiferEditor(editTarget, attachedLibrary);
        EditExistedQualifiers(editTarget);
    }

    private void NewQualiferEditor(List<Qualifier> editTarget, QualifierLibrary attachedLibrary) {
        int BUTTON_WIDTH = 20;
        string[] qualifierCandidates = attachedLibrary.GetAllQualifiersNames();
        EditorGUILayout.BeginHorizontal();
        selectIndex = EditorGUILayout.Popup(selectIndex, qualifierCandidates);
        if (GUILayout.Button("+", GUILayout.Width(BUTTON_WIDTH))) {
            // index 有效 且 修饰词未添加
            if (selectIndex < qualifierCandidates.Length && !editTarget.Exists((n)=> n.name ==qualifierCandidates[selectIndex])) {
                editTarget.Add(new Qualifier(qualifierCandidates[selectIndex]));
            }
        }
        EditorGUILayout.EndHorizontal();
    }

    private void EditExistedQualifiers(List<Qualifier> editTarget) {
        EditorGUILayout.BeginHorizontal();
        Qualifier toDelete = null;
        foreach (var qualifier in editTarget) {
            EditorGUILayout.LabelField(qualifier.name);
            if (GUILayout.Button("X",GUILayout.Width(20))) {
                toDelete = qualifier;
            }
        }
        EditorGUILayout.EndHorizontal();
        
        if (toDelete != null) {
            editTarget.Remove(toDelete);
        }
    }
}
