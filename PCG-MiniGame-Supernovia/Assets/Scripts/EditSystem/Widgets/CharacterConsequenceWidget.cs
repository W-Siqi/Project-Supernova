using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CharacterConsequenceWidget:Widget
{
    private static string[] bindingOptions ={ "随机","角色1","角色2","角色3"};
    
    private CharacterConsequence editTarget;
    private SelectAndAddWidget<QualiferAlteration> addQualiferAlterationWidget;
    private DynamicWidgetGroup<QualifierAlterationWidget, QualiferAlteration> qualierAlterationWidgetGroup;
    public CharacterConsequenceWidget(CharacterConsequence editTarget) {
        this.editTarget = editTarget;
        qualierAlterationWidgetGroup = new DynamicWidgetGroup<QualifierAlterationWidget, QualiferAlteration>(editTarget.qualiferAlterations);
        addQualiferAlterationWidget = new SelectAndAddWidget<QualiferAlteration>(
            editTarget.qualiferAlterations,
            GetAllPotencialQualiferCanAdd,
            QualiferAlteration.CreateByName);
    }
    public override void RenderUI() {
        EditorGUILayout.BeginHorizontal();
        editTarget.bindFlag = EditorGUILayout.Popup(editTarget.bindFlag, bindingOptions);
        addQualiferAlterationWidget.RenderUI();
        qualierAlterationWidgetGroup.RenderUI();
        EditorGUILayout.EndHorizontal();
    }

    string[] GetAllPotencialQualiferCanAdd() {
        var allNames = DeckArchive.instance.characterQualifierLib.GetAllQualifiersNames();
        var potiencialNames = new List<string>();
        foreach (var name in allNames) {
            if (!editTarget.qualiferAlterations.Exists((q) => q.targetQualifier.name == name)) {
                potiencialNames.Add(name);
            }
        }

        return potiencialNames.ToArray();
    }
}
