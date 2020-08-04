using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using PCG;

public class ConsequenceWidget : Widget{
    private ConsequenceSet editTarget;

    private StatusConsequenceWidget statusConsequenceWidget;
    private DynamicWidgetGroup<CharacterConsequenceWidget, CharacterConsequence> characaterConsequenceGroup;

    public ConsequenceWidget(ConsequenceSet editTarget) {
        this.editTarget = editTarget;

        // init  widgets
        statusConsequenceWidget = new StatusConsequenceWidget(editTarget.statusConsequence);
        characaterConsequenceGroup = new DynamicWidgetGroup<CharacterConsequenceWidget, CharacterConsequence>(editTarget.characterConsequences,false);
    }

    public override void RenderUI() {
        EditorGUILayout.Space();
        if (editTarget.statusConsequenceEnabled) {
            EditorGUILayout.BeginVertical(EditorStyleResource.consequenceBlockStyle);
            EditorGUILayout.LabelField("状态值 后果");
            statusConsequenceWidget.RenderUI();
            EditorGUILayout.EndVertical();
            EditorGUILayout.Space();
        }

        if (editTarget.characterConsequenceEnabled) {
            EditorGUILayout.BeginVertical(EditorStyleResource.consequenceBlockStyle);
            EditorGUILayout.LabelField("人物型后果");
            if (GUILayout.Button("Add")) {
                editTarget.characterConsequences.Add(new CharacterConsequence());
            }
            characaterConsequenceGroup.RenderUI();
            EditorGUILayout.EndVertical();
            EditorGUILayout.Space();
        }

        if (editTarget.keywordConsequenceEnabled) {
            EditorGUILayout.BeginVertical(EditorStyleResource.consequenceBlockStyle);
            EditorGUILayout.LabelField("关键词后果");
            editTarget.keywordConsequence.keyword = (KeywordConsequence.Keyword)EditorGUILayout.EnumPopup(editTarget.keywordConsequence.keyword);
            EditorGUILayout.EndVertical();
            EditorGUILayout.Space();
        }
    }

    public void SetMask(
        bool enableCharacterConsequence, 
        bool statusConsequence,
        bool keywordConsequence) {
        editTarget.characterConsequenceEnabled = enableCharacterConsequence;
        editTarget.statusConsequenceEnabled = statusConsequence;
        editTarget.keywordConsequenceEnabled = keywordConsequence; 
    }
}
