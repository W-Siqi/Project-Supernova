using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using PCG;
# if UNITY_EDITOR
public class SearchCardBlock {
    public enum SearchTarget {
        characterCard,
        stratagemCard,
        eventCard
    }

    public delegate void OnSearchSelect(Card selectedCard);

    private SearchTarget searchTarget;
    private OnSearchSelect onSearchSelect;
    private int selectIndex;
    public SearchCardBlock(SearchTarget searchTarget,OnSearchSelect onSearchSelect) {
        this.searchTarget = searchTarget;
        this.onSearchSelect = onSearchSelect;
        selectIndex = 0;
    }

    public void RenderUI() {
        int BUTTON_WIDTH = 20;
        List<string> allCardsNames = new List<string>();
        var candidates = GetSearchCandidates();
        foreach (var card in candidates) {
            allCardsNames.Add(card.name);
        }
        EditorGUILayout.BeginHorizontal();

        selectIndex = EditorGUILayout.Popup(
             selectIndex,
            allCardsNames.ToArray(),
            GUILayout.Width(EditorStyleResource.SEARCH_BAR_WIDTH));

        if (GUILayout.Button("Select", GUILayout.Width(BUTTON_WIDTH))) {
            var selectedCard = candidates[selectIndex];
            onSearchSelect(selectedCard);
        }
        EditorGUILayout.EndHorizontal();
    }

    private Card[] GetSearchCandidates() {
        Card[] candidates;
        switch (searchTarget) {
            case SearchTarget.characterCard:
                candidates = DeckArchive.instance.characterCards.ToArray();
                break;
            case SearchTarget.stratagemCard:
                candidates = DeckArchive.instance.stratagemCards.ToArray();
                break;
            case SearchTarget.eventCard:
                candidates = DeckArchive.instance.eventCards.ToArray();
                break;
            default:
                candidates = new Card[0];
                break;
        }
        return candidates;
    }
}
#endif