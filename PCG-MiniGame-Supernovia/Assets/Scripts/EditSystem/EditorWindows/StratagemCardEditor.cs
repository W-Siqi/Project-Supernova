using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PCG;

public class StratagemCardEditor : CardEditor
{
    private DescriptionBlock descriptionBlock;

    protected override void Init(Card editTarget) {
        base.Init(editTarget);
        descriptionBlock = new DescriptionBlock(editTarget as StratagemCard);
    }

    protected override void OnGUI() {
        base.OnGUI();
        descriptionBlock.RenderUI();
    }
}
