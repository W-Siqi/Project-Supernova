using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StratagemCardEditor : CardEditor
{
    PreconditionEditWidget preconditionEditWidget;
    protected override void Init(Card editTarget) {
        base.Init(editTarget);
        var editStratagemTarget = editTarget as StratagemCard;
        preconditionEditWidget = new PreconditionEditWidget(editStratagemTarget.preconditonSet);
    }

    protected override void OnGUI() {
        base.OnGUI();
        preconditionEditWidget.RenderUI();
    }
}
