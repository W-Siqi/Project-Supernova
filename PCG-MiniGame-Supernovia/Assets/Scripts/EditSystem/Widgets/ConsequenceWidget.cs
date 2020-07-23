using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ConsequenceWidget : Widget{
    private ConsequenceSet editTarget;

    private EnvConsequenceWidget envConsequenceWidget;
    private DynamicWidgetGroup<CharacterConsequenceWidget, CharacterConsequence> characaterConsequenceGroup;
    private bool enaleEnviromentConsequence;
    private bool enableCharacterConsequence;

    public ConsequenceWidget(ConsequenceSet editTarget) {
        this.editTarget = editTarget;

        // init character widgets
        characaterConsequenceGroup = new DynamicWidgetGroup<CharacterConsequenceWidget, CharacterConsequence>(editTarget.characterConsequences,false);
        // int env widgets
        envConsequenceWidget = new EnvConsequenceWidget(this.editTarget.environmentConsequence);
    }

    public override void RenderUI() {
        if (enableCharacterConsequence) {
            EditorGUILayout.LabelField("人物型后果");
            if (GUILayout.Button("Add")) {
                editTarget.characterConsequences.Add(new CharacterConsequence());
            }
            characaterConsequenceGroup.RenderUI();
        }

        if (enaleEnviromentConsequence) {
            EditorGUILayout.LabelField("环境结果");
            envConsequenceWidget.RenderUI();
        }
    }

    public void SetMask(bool enableCharacterConsequence, bool enaleEnviromentConsequence) {
        this.enableCharacterConsequence = enableCharacterConsequence;
        foreach (var conseq in editTarget.characterConsequences) {
            conseq.enabled = enableCharacterConsequence;
        }
     
        this.enaleEnviromentConsequence = enaleEnviromentConsequence;
        editTarget.environmentConsequence.enabled = enaleEnviromentConsequence;       
    }
}
