using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EventCardEditor : CardEditor
{
    PreconditionEditWidget preconditionEditWidget;

    protected override void OnGUI() {
        base.OnGUI();
        preconditionEditWidget.RenderUI();
    }

    protected override void Init(Card editTarget) {
        base.Init(editTarget);
        var editEventTarget = editTarget as EventCard;
        preconditionEditWidget = new PreconditionEditWidget(editEventTarget.preconditonSet);
    }
}
