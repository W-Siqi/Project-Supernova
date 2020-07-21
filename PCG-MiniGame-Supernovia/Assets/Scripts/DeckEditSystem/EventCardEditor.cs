using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EventCardEditor : CardEditor
{
    protected override void OnGUI() {
        base.OnGUI();
        var editEventTarget = editTarget as EventCard;
        PreconditonEdit(editEventTarget);
    }

    // 前置条件编辑
    private void PreconditonEdit(EventCard eventCard) {
        CharacterPreconditionEdit(eventCard);
        EnvironmentPreconditonEdit(eventCard);
        EnvironmentPreconditonEdit(eventCard);
    }

    private void CharacterPreconditionEdit(EventCard eventCard) {
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("前置人物");
        if (GUILayout.Button("+")) {

        }
        EditorGUILayout.EndHorizontal();
    }


    private void EnvironmentPreconditonEdit(EventCard eventCard) { 
    }

    private void EventPreconditonEdit(EventCard eventCard) {
    }

    // 后置结果编辑
    private void ConsequenceEdit(EventCard eventCard) { 
    
    }
}
