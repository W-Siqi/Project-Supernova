using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using PCG;

public class CharacterDeckEditor : DeckEditor {
    [MenuItem("PCG编辑/卡组编辑/人物卡")]
    public static void ShowWindow() {
        var instance = (CharacterDeckEditor)EditorWindow.GetWindow(typeof(CharacterDeckEditor));
    }

    protected override void OnGUI() {
        base.OnGUI();
    }

    protected override Card[] GetCardsInDeck() {
        return DeckArchive.instance.GetCards(typeof(CharacterCard));
    }

    protected override Card CreateCardInDeck() {
        return new CharacterCard();
    }
}
