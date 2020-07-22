using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ConsequenceWidget{
    private ConsequenceSet editTarget;

    private EnvConsequenceWidget envConsequenceWidget;
    private DynamicWidgetGroup<CharacterConsequenceWidget, CharacterConsequence> characaterConsequenceGroup;
    public ConsequenceWidget(ConsequenceSet editTarget) {
        this.editTarget = editTarget;

        // init character widgets
        characaterConsequenceGroup = new DynamicWidgetGroup<CharacterConsequenceWidget, CharacterConsequence>(editTarget.characterConsequences,false);
        // int env widgets
        envConsequenceWidget = new EnvConsequenceWidget(this.editTarget.environmentConsequence);
    }

    public void RenderUI() {
        EditorGUILayout.LabelField("人物型后果");
        if (GUILayout.Button("Add")) {
            editTarget.characterConsequences.Add(new CharacterConsequence());
        }
        characaterConsequenceGroup.RenderUI();
        EditorGUILayout.LabelField("环境结果");
        envConsequenceWidget.RenderUI();
    }
}
