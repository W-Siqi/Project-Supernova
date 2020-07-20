using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventGenerator{
    public static EventCard[] GenrateEvents(Deck<EventCard> eventDeck,Kindom.State kindomState) {
        return eventDeck.GetAll();
    }
}
