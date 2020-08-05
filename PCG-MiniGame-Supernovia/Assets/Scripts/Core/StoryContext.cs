using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PCG;

// 用于保存一切故事运作信息的单例
public class StoryContext : MonoBehaviour {
    // 因为经常要切换上下文（副本卡），以及本中本
    // 需要维护一个deck栈，栈帧保存的是可用的卡的对象（只是引用，不追踪状态）
    private class StoryStackFrame {
        public CharacterCard[] characterCards;
        public StratagemCard[] stratagemCards;
        public EventCard[] eventCards;

        public StoryStackFrame(List<CharacterCard> characterCards, List<StratagemCard> stratagemCards, List<EventCard> eventCards) {
            this.characterCards = characterCards.ToArray();
            this.stratagemCards = stratagemCards.ToArray();
            this.eventCards = eventCards.ToArray();
        }
    }


    static StoryContext _instance = null;

    public List<CharacterCard> characterDeck = new List<CharacterCard>();
    public List<EventCard> eventDeck = new List<EventCard>();
    public Dictionary<CharacterCard, List<StratagemCard>> stratagemDict = new Dictionary<CharacterCard, List<StratagemCard>>();
    public StatusVector statusVector = new StatusVector();
    public List<Qualifier> environmentQualifiers = new List<Qualifier>();

    // 故事栈只管Deck里有哪些卡，不管状态。也就是环境状态这些是全局的
    private Stack<StoryStackFrame> storyStack = new Stack<StoryStackFrame>();

    public static StoryContext instance {
        get {
            if (_instance == null) {
                var GO = new GameObject("Story Context");
                _instance = GO.AddComponent<StoryContext>();
            }
            return _instance;
        }
    }


    public void InitForNewStory(int randomSeed) {
        var varTable = PCGVariableTable.instance;
        // init character
        for (int i = 0; i < varTable.characterCount; i++) {
            var charaPrototype = DeckArchive.instance.characterCards[i];
            var newCharacter = Card.DeepCopy(charaPrototype);
            // random properties
            newCharacter.loyalty = Random.Range(3, 7);
            foreach (var p in newCharacter.personalities) {
                p.trait = TraitUtils.GetRandomTrait();
            }

            characterDeck.Add(newCharacter);
        }

        // init startgems of character
        foreach (var chara in characterDeck) {
            stratagemDict[chara] = new List<StratagemCard>();
            for (int i = 0; i < varTable.roundCount; i++) {
                var stratagemPrototype = DeckArchive.instance.stratagemCards[Random.Range(0, DeckArchive.instance.stratagemCards.Count)];
                stratagemDict[chara].Add(Card.DeepCopy(stratagemPrototype));
            }
        }

        // init event
        eventDeck = new List<EventCard>();
        foreach (var card in DeckArchive.instance.eventCards) {
            eventDeck.Add(Card.DeepCopy(card));
        }

        // init status
        statusVector.army = Random.Range(20, 100);
        statusVector.money = Random.Range(20, 100);
        statusVector.people = Random.Range(20, 100);
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
