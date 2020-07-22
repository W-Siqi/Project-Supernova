using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EventCardEditor : CardEditor
{
    PreconditionEditWidget preconditionEditWidget;
    ConsequenceWidget consequenceWidget;

    protected override void OnGUI() {
        base.OnGUI();
        preconditionEditWidget.RenderUI();
        consequenceWidget.RenderUI();
    }

    protected override void Init(Card editTarget) {
        base.Init(editTarget);
        var editEventTarget = editTarget as EventCard;
        preconditionEditWidget = new PreconditionEditWidget(editEventTarget.preconditonSet);
        consequenceWidget = new ConsequenceWidget(editEventTarget.consequenceSet);
    }
}
