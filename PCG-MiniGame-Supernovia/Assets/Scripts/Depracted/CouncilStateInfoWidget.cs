# if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using PCG;

public class CouncilStateInfoWidget : Widget {
    private SerializedStory.CouncilStageInfo editTarget;
    private SearchCardBlock searchCharacterCardBlock;
    private SearchCardBlock searchStratagemCardBlock;
    private DynamicWidgetGroup<CardReferenceWidget, StratagemCard> stratagemCardWidgets;
    public CouncilStateInfoWidget(SerializedStory.CouncilStageInfo editTarget){
        this.editTarget = editTarget;
        searchCharacterCardBlock = new SearchCardBlock(
            SearchCardBlock.SearchTarget.characterCard,
            (selected) => { editTarget.characterCard = selected as CharacterCard; });
        searchStratagemCardBlock = new SearchCardBlock(
            SearchCardBlock.SearchTarget.stratagemCard,
            (selected) => { editTarget.stratagemCards.Add(selected as StratagemCard); });
        stratagemCardWidgets = new DynamicWidgetGroup<CardReferenceWidget, StratagemCard>(editTarget.stratagemCards);
    }

    public override void RenderUI() {
        EditorGUILayout.BeginVertical();
        EditorGUILayout.BeginVertical(EditorStyleResource.consequenceBlockStyle);
        searchCharacterCardBlock.RenderUI();
        if (editTarget.characterCard!=null) {
            var widget = new CardReferenceWidget(editTarget.characterCard);
            widget.RenderUI();
        }

        searchStratagemCardBlock.RenderUI();
        stratagemCardWidgets.RenderUI();

        EditorGUILayout.EndVertical();
        EditorGUILayout.Space();
        EditorGUILayout.EndVertical();
    }
}
#endif