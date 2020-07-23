using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EventCardEditor : CardEditor
{
    private DescriptionBlock descriptionBlock;

    protected override void OnGUI() {
        base.OnGUI();
        descriptionBlock.RenderUI();
    }

    protected override void Init(Card editTarget) {
        base.Init(editTarget);
        descriptionBlock = new DescriptionBlock(editTarget as EventCard);
    }
}
