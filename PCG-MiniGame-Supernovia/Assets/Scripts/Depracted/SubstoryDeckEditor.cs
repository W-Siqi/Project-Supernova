using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using PCG;

public class SubstoryDeckEditor : DeckEditor {
    //[MenuItem("PCG编辑/子故事编辑器")]
    public static void ShowWindow() {
        var instance = (SubstoryDeckEditor)EditorWindow.GetWindow(typeof(SubstoryDeckEditor));
    }

    protected override void OnGUI() {
        base.OnGUI();
    }

    protected override Card[] GetCardsInDeck() {
        return DeckArchive.instance.GetCards(typeof(SubstoryCard));
    }

    protected override Card CreateCardInDeck() {
        return new SubstoryCard();
    }
}
