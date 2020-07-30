using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using PCG;
public class EventDeckEditor : DeckEditor
{
    [MenuItem("PCG编辑/卡组编辑/事件卡")]
    public static void ShowWindow() {
        var instance = (EventDeckEditor)EditorWindow.GetWindow(typeof(EventDeckEditor));
    }

    protected override void OnGUI() {
        base.OnGUI();
    }

    protected override Card[] GetCardsInDeck() {
        return DeckArchive.instance.GetCards(typeof(EventCard));
    }

    protected override Card CreateCardInDeck() {
        return new EventCard();
    }
}
