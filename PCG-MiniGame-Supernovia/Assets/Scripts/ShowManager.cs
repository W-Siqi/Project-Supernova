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

    /// <summary>
    /// 副本/支线 卡到来时候的动画 (必须要在StoryContext还没变之前调用！)
    /// </summary>
    /// <returns></returns>
    public IEnumerator SubstoryArriveShow(SubstoryCard substoryCard) {
        var spawnAnchor = AnchorManager.instance.deckSpawnAnchor;

        var storyCardDisplay = CardDisplayBehaviour.Create(
            substoryCard,
            spawnAnchor.transform.position,
            spawnAnchor.transform.rotation);
        yield return new WaitForSeconds(1.5f);
        DestroyImmediate(storyCardDisplay.gameObject);

        if (substoryCard.type == SubstoryCard.Type.dungeon) {
            var cardsShuffleOut = new List<Card>();
            cardsShuffleOut.AddRange(StoryContext.instance.eventDeck);
            cardsShuffleOut.AddRange(StoryContext.instance.stratagemDeck);
            yield return StartCoroutine(PlayCardsShuffleOut(cardsShuffleOut.ToArray()));
        }

        var cardsShuffleIn = new List<Card>();
        cardsShuffleIn.AddRange(substoryCard.newCharacters);
        cardsShuffleIn.AddRange(substoryCard.eventCards);
        cardsShuffleIn.AddRange(substoryCard.stratagemCards);
        yield return StartCoroutine(PlayCardsShuffleIn(cardsShuffleIn.ToArray()));

        yield return new WaitForSeconds(1f);
    }

    /// <summary>
    /// 把卡片全部吸进牌组
    /// </summary>
    /// <returns></returns>
    public IEnumerator PlayCardsShuffleIn(Card[] cards) {
        var anchor = AnchorManager.instance.deckSpawnAnchor;
        Vector3 startPos = anchor.transform.position;
        Quaternion startRotation = anchor.transform.rotation;
        foreach (var card in cards) {
            DeckTarget belongedDeck;
            if (card is CharacterCard) {
                belongedDeck = DeckTarget.characterDeck;
            }
            else if (card is EventCard) {
                belongedDeck = DeckTarget.eventDeck;
            }
            else {
                belongedDeck = DeckTarget.stratagemDeck;
            }
            var cardDisplay = CardDisplayBehaviour.Create(card, startPos, startRotation);
            BackCardToDeck(cardDisplay, belongedDeck);
            yield return new WaitForSeconds(0.5f);
        }

        yield return new WaitForSeconds(1f);
    }

    /// <summary>
    /// 把当前deck里面的排全部洗出去的动画
    /// </summary>
    /// <returns></returns>
    public IEnumerator PlayCardsShuffleOut(Card[] cards) {
        var anchor = AnchorManager.instance.deckSpawnAnchor;
        foreach (var card in cards) {
            DeckTarget belongedDeck;
            if (card is CharacterCard) {
                belongedDeck = DeckTarget.characterDeck;
            }
            else if (card is EventCard) {
                belongedDeck = DeckTarget.eventDeck;
            }
            else {
                belongedDeck = DeckTarget.stratagemDeck;
            }

            var cardDisplay = ShowCardFromDeck(card, belongedDeck,anchor);
            Destroy(cardDisplay.gameObject, 5f);
            yield return new WaitForSeconds(0.2f);
        }
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

    /// <summary>
    /// 卡片从牌堆抽出来到某个位置的动画
    /// </summary>
    /// <param name="card"></param>
    /// <param name="from"></param>
    /// <param name="anchorPoint"></param>
    /// <returns></returns>
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

    /// <summary>
    /// 把卡片洗回去牌堆的动画
    /// </summary>
    /// <param name="cardDisplayBehaviour"></param>
    /// <param name="deck"></param>
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
