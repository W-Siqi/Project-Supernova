using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using PCG;


public class CharacterPreconditonWidget : Widget
{
    private CharacterPrecondition editTarget;

    public CharacterPreconditonWidget(CharacterPrecondition editTarget) {
        this.editTarget = editTarget;
    }

    public override void RenderUI() {
        EditorGUILayout.BeginHorizontal();
        editTarget.requiredTrait = (Trait)EditorGUILayout.EnumPopup(editTarget.requiredTrait);
        EditorGUILayout.LabelField("误差允许度", GUILayout.Width(100));
        EditorGUILayout.EndHorizontal();
    }
}
