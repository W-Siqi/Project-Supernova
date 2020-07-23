using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CharacterPreconditonWidget : Widget
{
    private CharacterPrecondition editTarget;
    private SelectAndAddWidget<Qualifier> addQualiferWidget;
    private DynamicWidgetGroup<QualifierWidget, Qualifier> qualifierWidgetGroups;

    public CharacterPreconditonWidget(CharacterPrecondition editTarget) {
        this.editTarget = editTarget;
        qualifierWidgetGroups = new DynamicWidgetGroup<QualifierWidget, Qualifier>(editTarget.qualifiers);
        addQualiferWidget = new SelectAndAddWidget<Qualifier>(
            editTarget.qualifiers,
            ()=>DeckArchive.instance.characterQualifierLib.GetQualiferNamesWithBlackList(editTarget.qualifiers),
            (string name)=>new Qualifier(name));
    }

    public override void RenderUI() {
        EditorGUILayout.BeginHorizontal();
        addQualiferWidget.RenderUI();
        qualifierWidgetGroups.RenderUI();
        EditorGUILayout.EndHorizontal();
    }
}
