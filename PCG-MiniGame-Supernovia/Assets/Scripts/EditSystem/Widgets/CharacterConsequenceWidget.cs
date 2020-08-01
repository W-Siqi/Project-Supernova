using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using PCG;

public class CharacterConsequenceWidget:Widget
{
    private static string[] bindingOptions ={ "角色1","角色2","角色3"};
    
    private CharacterConsequence editTarget;
    public CharacterConsequenceWidget(CharacterConsequence editTarget) {
        this.editTarget = editTarget;
    }
    public override void RenderUI() {
        EditorGUILayout.BeginHorizontal();
        editTarget.bindFlag = EditorGUILayout.Popup(editTarget.bindFlag, bindingOptions);
        editTarget.traitToBecome = (Trait)EditorGUILayout.EnumPopup(editTarget.traitToBecome);
        EditorGUILayout.LabelField("转化强度", GUILayout.Width(100));
        editTarget.transferStrength = EditorGUILayout.IntField(editTarget.transferStrength);
        EditorGUILayout.EndHorizontal();
    }
}
