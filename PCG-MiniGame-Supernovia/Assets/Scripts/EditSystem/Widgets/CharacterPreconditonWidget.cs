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
        editTarget.isRandom = EditorGUILayout.Toggle("随机对象", editTarget.isRandom);
        if (!editTarget.isRandom) {
            editTarget.requiredTrait = (Trait)EditorGUILayout.EnumPopup(editTarget.requiredTrait);
        }
        EditorGUILayout.EndHorizontal();
    }
}
