using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 用于保存一切故事运作信息的单例
public class StoryContext{
    [System.Serializable]
    public class KindomStateValue {
        public int army = 1;
        public int financy = 1;
        public int luck = 1;
    }
    
   private static StoryContext _instance = null;

    public List<CharacterCard> characterDeck = new List<CharacterCard>();
    public List<StratagemCard> stratagemDeck = new List<StratagemCard>();
    public List<EventCard> eventDeck = new List<EventCard>();
    public KindomStateValue kindomState = new KindomStateValue();
    public List<Qualifier> environmentQualifiers = new List<Qualifier>();

    public static StoryContext instance {
        get {
            if (_instance == null) {
                _instance = new StoryContext();
            }
            return _instance;
        }
    }

    public void InitForNewStory(int randomSeed) {
        DealStartingDecks(out characterDeck, out stratagemDeck, out eventDeck);
    }

    private static void DealStartingDecks(
       out List<CharacterCard> characterDeck,
       out List<StratagemCard> stratagemDeck,
       out List<EventCard> eventDeck) {
        characterDeck = new List<CharacterCard>();
        foreach (var card in DeckArchive.instance.characterCards) {
            characterDeck.Add(Card.DeepCopy(card));
        }

        stratagemDeck = new List<StratagemCard>();
        foreach (var card in DeckArchive.instance.stratagemCards) {
            stratagemDeck.Add(Card.DeepCopy(card));
        }

        eventDeck = new List<EventCard>();
        foreach (var card in DeckArchive.instance.eventCards) {
            eventDeck.Add(Card.DeepCopy(card));
        }
    }
}
