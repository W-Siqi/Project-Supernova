using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using PCG;

# if UNITY_EDITOR
public class StratagemDeckEditor : DeckEditor
{
    [MenuItem("PCG编辑/决策卡")]
    public static void ShowWindow() {
        var instance = (StratagemDeckEditor)EditorWindow.GetWindow(typeof(StratagemDeckEditor));
    }
    protected override void OnGUI() {
        base.OnGUI();
    }

    protected override Card[] GetCardsInDeck() {
        return DeckArchive.instance.GetCards(typeof(StratagemCard));
    }

    protected override Card CreateCardInDeck() {
        return new StratagemCard();
    }
}
# endif