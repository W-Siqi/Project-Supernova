using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using PCG;

public class SerializedStorySectionWidget : Widget
{
    private SerializedStory.Section editTarget;
    private DynamicWidgetGroup<CouncilStateInfoWidget, SerializedStory.CouncilStageInfo> countilStateInfoWidgets;
    private SearchCardBlock searchEventBlcok;
    private DynamicWidgetGroup<CardReferenceWidget, EventCard> eventCardWidgets;
    private DynamicWidgetGroup<ShowInfoWidget, ShowInfo> showInfoWidgets;

    public SerializedStorySectionWidget(SerializedStory.Section editTarget) {
        // council stage
        this.editTarget = editTarget;
        countilStateInfoWidgets = new DynamicWidgetGroup<CouncilStateInfoWidget, SerializedStory.CouncilStageInfo>(editTarget.councilStageInfos,false);
        
        // event
        eventCardWidgets = new DynamicWidgetGroup<CardReferenceWidget, EventCard>(editTarget.eventCards);
        searchEventBlcok = new SearchCardBlock(
            SearchCardBlock.SearchTarget.eventCard,
            (selected) => { editTarget.eventCards.Add(selected as EventCard); });

        // show infos
        showInfoWidgets = new DynamicWidgetGroup<ShowInfoWidget, ShowInfo>(editTarget.showInfos);
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

        // show infos
        if (GUILayout.Button("+Show Info")) {
            editTarget.showInfos.Add(new ShowInfo());
        }
        showInfoWidgets.RenderUI();
        EditorGUILayout.EndVertical();
    }
}
