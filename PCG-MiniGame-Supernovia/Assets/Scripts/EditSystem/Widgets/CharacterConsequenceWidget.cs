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
        editTarget.traitAlteration.targetTrait = (Trait)EditorGUILayout.EnumPopup(editTarget.traitAlteration.targetTrait);
        editTarget.traitAlteration.type = (TraitAlteration.Type)EditorGUILayout.EnumPopup(editTarget.traitAlteration.type);
        editTarget.loyaltyAlteraion = EditorGUILayout.IntField(editTarget.loyaltyAlteraion);
        EditorGUILayout.EndHorizontal();
    }
}
