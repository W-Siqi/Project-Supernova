using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EventPrecondition : Precondition
{
    public string eventCardName = "";

    public EventPrecondition(EventCard eventCard) {
        eventCardName = eventCard.name;
    }
}
