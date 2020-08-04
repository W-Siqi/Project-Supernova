using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PCG;

// 用于保存一切故事运作信息的单例
public class StoryContext : MonoBehaviour {
    [System.Serializable]
    public class StatusVector {
        public int people = 80;
        public int money = 76;
        public int army= 20;

        public static StatusVector operator + (StatusVector a, StatusVector b) {
            var res = new StatusVector();
            res.people = a.people + b.people;
            res.money = a.money + b.money;
            res.army = a.army + b.army;
            return res;
        }
    }

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
    public List<StratagemCard> stratagemDeck = new List<StratagemCard>();
    public List<EventCard> eventDeck = new List<EventCard>();
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

    /// <summary>
    /// 保存当前上下文，并切换到substory对应的上下文
    /// </summary>
    /// <param name="substoryCard"></param>
    public void PushSubstory(SubstoryCard substoryCard) {
        // 把当前故事压栈
        var curFrame = new StoryStackFrame(characterDeck,stratagemDeck,eventDeck);
        storyStack.Push(curFrame);

        // 替换当前
        var allCharacters = new List<CharacterCard>();
        allCharacters.AddRange(characterDeck);
        allCharacters.AddRange(substoryCard.newCharacters);
        this.characterDeck = new List<CharacterCard>(allCharacters);
        this.stratagemDeck = new List<StratagemCard>(substoryCard.stratagemCards);
        this.eventDeck = new List<EventCard>(substoryCard.eventCards);
    }

    public void PopSubstory() {
        if (storyStack.Count <= 0) {
            Debug.LogError("故事栈为空，不能再出");
            return;
        }

        var top = storyStack.Pop();
        this.characterDeck = new List<CharacterCard>(top.characterCards);
        this.stratagemDeck = new List<StratagemCard>(top.stratagemCards);
        this.eventDeck = new List<EventCard>(top.eventCards);
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
