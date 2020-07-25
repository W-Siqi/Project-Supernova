using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CharacterDeckEditor : DeckEditor {
    [MenuItem("卡牌编辑器/人物卡组")]
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
