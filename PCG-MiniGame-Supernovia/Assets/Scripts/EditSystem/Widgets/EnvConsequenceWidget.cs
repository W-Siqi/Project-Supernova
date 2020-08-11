using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

# if UNITY_EDITOR
public class EnvConsequenceWidget : Widget
{
    private EnvironmentConsequence editTarget;
    private SelectAndAddWidget<QualiferAlteration> addQualiferAlterationWidget;
    private DynamicWidgetGroup<QualifierAlterationWidget, QualiferAlteration> widgetGroup;

    public EnvConsequenceWidget(EnvironmentConsequence editTarget) {
        this.editTarget = editTarget;
        widgetGroup = new DynamicWidgetGroup<QualifierAlterationWidget, QualiferAlteration>(editTarget.qualiferAlterations);
        addQualiferAlterationWidget = new SelectAndAddWidget<QualiferAlteration>(
            editTarget.qualiferAlterations,
            GetAllPotencialQualiferCanAdd,
            QualiferAlteration.CreateByName);
    }

    public override void RenderUI() {
        addQualiferAlterationWidget.RenderUI();
        widgetGroup.RenderUI();
    }

    string[] GetAllPotencialQualiferCanAdd() {
        var allNames = DeckArchive.instance.environmentQualifierLib.GetAllQualifiersNames();
        var potiencialNames = new List<string>();
        foreach (var name in allNames) {
            if (!editTarget.qualiferAlterations.Exists((q) => q.targetQualifier.name == name)) {
                potiencialNames.Add(name);
            }
        }

        return potiencialNames.ToArray();
    }
}
#endif