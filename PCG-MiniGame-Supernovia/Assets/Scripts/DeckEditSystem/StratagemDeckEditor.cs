using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class StratagemDeckEditor : DeckEditor
{
    [MenuItem("卡牌编辑器/决策卡组")]
    public static void ShowWindow() {
        var instance = (StratagemDeckEditor)EditorWindow.GetWindow(typeof(StratagemDeckEditor));
        instance.cardTypeInDeck = typeof(StratagemCard);
    }

    protected override void OnGUI() {
        base.OnGUI();
    }
}
