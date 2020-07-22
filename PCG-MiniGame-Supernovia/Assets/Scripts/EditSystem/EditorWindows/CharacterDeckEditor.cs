using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CharacterDeckEditor : DeckEditor {
    [MenuItem("卡牌编辑器/人物卡组")]
    public static void ShowWindow() {
        var instance = (CharacterDeckEditor)EditorWindow.GetWindow(typeof(CharacterDeckEditor));
        instance.cardTypeInDeck = typeof(CharacterCard);
    }

    protected override void OnGUI() {
        base.OnGUI();
    }
}
