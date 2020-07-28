using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SerializedStorySectionWidget : Widget
{
    private SerializedStory.Section editTarget;
    private DynamicWidgetGroup<CouncilStateInfoWidget, SerializedStory.CouncilStageInfo> countilStateInfoWidgets;
    private SearchCardBlock searchEventBlcok;
    private DynamicWidgetGroup<CardReferenceWidget, EventCard> eventCardWidgets;
    private CardReferenceWidget substoryCardWidget;

    public SerializedStorySectionWidget(SerializedStory.Section editTarget) {
        this.editTarget = editTarget;
        countilStateInfoWidgets = new DynamicWidgetGroup<CouncilStateInfoWidget, SerializedStory.CouncilStageInfo>(editTarget.councilStageInfos,false);
        eventCardWidgets = new DynamicWidgetGroup<CardReferenceWidget, EventCard>(editTarget.eventCards);
        searchEventBlcok = new SearchCardBlock(
            SearchCardBlock.SearchTarget.eventCard,
            (selected) => { editTarget.eventCards.Add(selected as EventCard); });
    }

    public override void RenderUI() {
        // councial section
        EditorGUILayout.BeginVertical();
        if (GUILayout.Button("+Councial Info")) {
            editTarget.councilStageInfos.Add(new SerializedStory.CouncilStageInfo());
        }
        countilStateInfoWidgets.RenderUI();

        // evenet section
        EditorGUILayout.Space();
        EditorGUILayout.BeginVertical(EditorStyleResource.preconditionBlockStyle);
        searchEventBlcok.RenderUI();
        eventCardWidgets.RenderUI();
        EditorGUILayout.EndVertical();

        // subsutory secetion
        if (substoryCardWidget != null) {
            substoryCardWidget.RenderUI();
        }
        EditorGUILayout.EndVertical();
    }
}
