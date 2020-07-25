using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EventDeckEditor : DeckEditor
{
    [MenuItem("卡牌编辑器/事件卡组")]
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
