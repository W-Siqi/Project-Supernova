using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ConsequenceWidget : Widget{
    private ConsequenceSet editTarget;

    private EnvConsequenceWidget envConsequenceWidget;
    private FightConsequenceWidget fightConsequenceWidget;
    private DynamicWidgetGroup<CharacterConsequenceWidget, CharacterConsequence> characaterConsequenceGroup;

    public ConsequenceWidget(ConsequenceSet editTarget) {
        this.editTarget = editTarget;

        // init  widgets
        characaterConsequenceGroup = new DynamicWidgetGroup<CharacterConsequenceWidget, CharacterConsequence>(editTarget.characterConsequences,false);
        envConsequenceWidget = new EnvConsequenceWidget(this.editTarget.environmentConsequence);
        fightConsequenceWidget = new FightConsequenceWidget(this.editTarget.fightConsequence);
    }

    public override void RenderUI() {
        EditorGUILayout.Space();
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

        if (editTarget.environmentConsequenceEnabled) {
            EditorGUILayout.BeginVertical(EditorStyleResource.consequenceBlockStyle);
            EditorGUILayout.LabelField("环境结果");
            envConsequenceWidget.RenderUI();
            EditorGUILayout.EndVertical();
            EditorGUILayout.Space();
        }

        if (editTarget.fightConsequenceEnabled) {
            EditorGUILayout.BeginVertical(EditorStyleResource.consequenceBlockStyle);
            EditorGUILayout.LabelField("战斗后果");
            fightConsequenceWidget.RenderUI();
            EditorGUILayout.EndVertical();
            EditorGUILayout.Space();
        }
    }

    public void SetMask(bool enableCharacterConsequence, bool enaleEnviromentConsequence, bool enableFightConsequence) {
        editTarget.characterConsequenceEnabled = enableCharacterConsequence;
        editTarget.environmentConsequenceEnabled = enaleEnviromentConsequence;
        editTarget.fightConsequenceEnabled = enableFightConsequence;
    }
}
