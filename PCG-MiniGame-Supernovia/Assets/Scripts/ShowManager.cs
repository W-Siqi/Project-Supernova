using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 负责一切的卡牌动画，和演出效果
public class ShowManager : MonoBehaviour {
    public enum DeckTarget{
        characterDeck,
        eventDeck,
        stratagemDeck
    }

    // 用来后续跟踪，并进行后续动画的一个标记
    public class TrackingHandle {
        public DeckDisplayBehaviour belongedDeck;
    }

    // singaleton
    private static ShowManager _instance = null;
    public static ShowManager instance {
        get {
            if (_instance == null) {
                _instance = FindObjectOfType<ShowManager>();
            }
            return _instance;
        }
    }

    [SerializeField]
    private DeckDisplayBehaviour characterDeckDisplay;
    [SerializeField]
    private DeckDisplayBehaviour eventDeckDisplay;
    [SerializeField]
    private DeckDisplayBehaviour stratagemDeckDisplay;

    private Dictionary<TrackingHandle, CardDisplayBehaviour> trackingDict = new Dictionary<TrackingHandle, CardDisplayBehaviour>();

    // 游戏开始时洗牌演出
    public IEnumerator PlayDeckInitShuffle() {
        var anchor = AnchorManager.instance.deckSpawnAnchor;
        Vector3 startPos = anchor.transform.position;
        Quaternion startRotation = anchor.transform.rotation;

        var chaCards = new List<CardDisplayBehaviour>();
        var straCards = new List<CardDisplayBehaviour>();
        var eveCards = new List<CardDisplayBehaviour>();
        foreach (var src in StoryContext.instance.characterDeck) {
            chaCards.Add(CardDisplayBehaviour.Create(src, startPos, startRotation));
        }
        foreach (var src in StoryContext.instance.eventDeck) {
            eveCards.Add(CardDisplayBehaviour.Create(src, startPos, startRotation));
        }
        foreach (var src in StoryContext.instance.stratagemDeck) {
            straCards.Add(CardDisplayBehaviour.Create(src, startPos, startRotation));
        }

        yield return StartCoroutine(CardsToDeck(chaCards, characterDeckDisplay));
        yield return StartCoroutine(CardsToDeck(straCards, eventDeckDisplay));
        yield return StartCoroutine(CardsToDeck(eveCards, stratagemDeckDisplay));

        yield return new WaitForSeconds(1f);
    }

    public TrackingHandle ShowCardFromDeck(Card card,DeckTarget from, AnchorPoint anchorPoint) {
        DeckDisplayBehaviour belongedDeck;
        switch (from) {
            case DeckTarget.characterDeck:
                belongedDeck = characterDeckDisplay;
                break;
            case DeckTarget.stratagemDeck:
                belongedDeck = stratagemDeckDisplay;
                break;
            case DeckTarget.eventDeck:
                belongedDeck = eventDeckDisplay;
                break;
            default:
                belongedDeck = characterDeckDisplay;
                break;
        }

        Vector3 topPos;
        Quaternion topRotation;
        belongedDeck.DrawCard(out topPos,out topRotation);
        var cardDisplay =  CardDisplayBehaviour.Create(card, topPos, topRotation);
        LerpAnimator.instance.LerpPositionAndRotation(
            cardDisplay.transform,
            anchorPoint.transform.position,
            anchorPoint.transform.rotation,
            1f);

        var trackingHandle = new TrackingHandle();
        trackingHandle.belongedDeck = belongedDeck;
        trackingDict[trackingHandle] = cardDisplay;

        return trackingHandle;
    }

    public void BackCardToDeck(TrackingHandle trackingHandleOfCard) {
        var targetCardDisplay = trackingDict[trackingHandleOfCard];
        PlayCardToDeck(targetCardDisplay,trackingHandleOfCard.belongedDeck,1f);
    }

    IEnumerator CardsToDeck(List<CardDisplayBehaviour>cards, DeckDisplayBehaviour deck) {
        foreach (var c in cards){
            PlayCardToDeck(c, deck, 1f);
            yield return new WaitForSeconds(0.2f);
        }
    }

    void PlayCardToDeck(CardDisplayBehaviour card, DeckDisplayBehaviour deck, float playTime) {
            var deckTopPos = deck.deckTopPos;
            var deckRotate = deck.transform.rotation;
            LerpAnimator.instance.LerpPositionAndRotation(
                card.transform,
                deckTopPos,
                deckRotate,
                playTime,
                () => { DestroyImmediate(card.gameObject); deck.AddCard(); });
    }
}
