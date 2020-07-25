using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 负责一切的卡牌动画，和演出效果
public class ShowManager : MonoBehaviour {
    public enum DeckTarget {
        characterDeck,
        eventDeck,
        stratagemDeck
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

    // 各种事件卡的万能入口
    public  IEnumerator ShowEvent(EventCard eventCard, CharacterCard[] bindedCharacters) {
        // show event card selfs
        var eventCardDisplay = ShowCardFromDeck( eventCard, DeckTarget.eventDeck,AnchorManager.instance.eventCardAnchor);
        yield return new WaitForSeconds(1f);

        // 对战斗后果进行演出
        if (eventCard.consequenceSet.fightConsequenceEnabled) {
            var fightConseq = eventCard.consequenceSet.fightConsequence;
            var attacker = fightConseq.GetAttacker(bindedCharacters);
            var defender = fightConseq.GetDefender(bindedCharacters);

            var attackDisplay = ShowCardFromDeck(attacker, DeckTarget.characterDeck, AnchorManager.instance.showCardLeftAnchor);
            var defenderDisplay = ShowCardFromDeck(defender, DeckTarget.characterDeck, AnchorManager.instance.showCardRightAnchor);
            yield return new WaitForSeconds(2f);

            HitEffect.Create(attacker.attributes.atkVal, defenderDisplay.transform.position);
            yield return new WaitForSeconds(3f);

            BackCardToDeck(attackDisplay, DeckTarget.characterDeck);
            BackCardToDeck(defenderDisplay, DeckTarget.characterDeck);
        }

        BackCardToDeck(eventCardDisplay, DeckTarget.eventDeck);
        yield return new WaitForSeconds(2f);
    }


    public CardDisplayBehaviour ShowCardFromDeck(Card card,DeckTarget from, AnchorPoint anchorPoint) {
        DeckDisplayBehaviour belongedDeck = GetBlongedDeckDisplay(from);

        Vector3 topPos;
        Quaternion topRotation;
        belongedDeck.DrawCard(out topPos,out topRotation);
        var cardDisplay =  CardDisplayBehaviour.Create(card, topPos, topRotation);
        LerpAnimator.instance.LerpPositionAndRotation(
            cardDisplay.transform,
            anchorPoint.transform.position,
            anchorPoint.transform.rotation,
            1f);

        return cardDisplay;
    }

    public void BackCardToDeck(CardDisplayBehaviour cardDisplayBehaviour, DeckTarget deck) {
        PlayCardToDeck(cardDisplayBehaviour, GetBlongedDeckDisplay(deck),1f);
    }

    private DeckDisplayBehaviour GetBlongedDeckDisplay(DeckTarget deck) {
        DeckDisplayBehaviour belongedDeck;
        switch (deck) {
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
        return belongedDeck;
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
