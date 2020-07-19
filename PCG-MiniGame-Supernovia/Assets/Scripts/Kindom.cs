using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kindom : MonoBehaviour
{
    [System.Serializable]
    public class State { 
    
    }

    public State state;
    // control stratagm decision
    public Deck<CharacterCard> characterDeck;
    // event 
    public Deck<EventCard> eventDeck;

    public void ApplyEvent(EventCard eventCard) { 
    
    }

    public void ApplyStratagem(StratagemCard stratagemCard) { 
    
    }
}
