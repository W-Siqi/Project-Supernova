using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventCardDealer
{
    public static EventCard[] DealEvents(Deck<EventCard> eventDeck, Kindom.State kindomState)
    {
        return eventDeck.GetAll();
    }
}
