using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

# if UNITY_EDITOR
namespace PCG {
    public class StratagemCardEditor : CardEditor {
        private StatusConsequenceWidget statusConsequenceWidget;
        protected override void Init(Card editTarget) {
            base.Init(editTarget);
            var straCard = (StratagemCard)editTarget;
            statusConsequenceWidget = new StatusConsequenceWidget(straCard.consequenceSet.statusConsequenceWhenAccept);
        }

        protected override void OnGUI() {
            base.OnGUI();
            var stratagemCard = editTarget as StratagemCard;
            stratagemCard.yesText = EditorGUILayout.TextField("yes text", stratagemCard.yesText);
            stratagemCard.noText = EditorGUILayout.TextField("no text", stratagemCard.noText);
            statusConsequenceWidget.RenderUI();
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("接受");
            var traitAlter1 = stratagemCard.consequenceSet.traitAlterationWhenAccept;
            traitAlter1.targetTrait = (Trait) EditorGUILayout.EnumPopup(traitAlter1.targetTrait);
            traitAlter1.type = (TraitAlteration.Type)EditorGUILayout.EnumPopup(traitAlter1.type);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("拒绝");
            var traitAlter2 = stratagemCard.consequenceSet.traitAlterationWhenDecline;
            traitAlter2.targetTrait = (Trait)EditorGUILayout.EnumPopup(traitAlter2.targetTrait);
            traitAlter2.type = (TraitAlteration.Type)EditorGUILayout.EnumPopup(traitAlter2.type);
            EditorGUILayout.EndHorizontal();

        }
    }
}

#endif