using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PCG;

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
    /// 把卡片全部吸进牌组
    /// </summary>
    /// <returns></returns>
    public IEnumerator PlayCardsShuffleIn(Card[] cards) {
        var anchor = AnchorManager.instance.deckSpawnAnchor;
        var randDistance = 0.2f;
        //var randRotateAngle = 30f;

        var instanciteCards = new List<KeyValuePair<CardDisplayBehaviour, DeckTarget>>();
        // spwan init 
        foreach (var card in cards) {
            // spwan
            var spawnPos = anchor.transform.position + randDistance * Random.insideUnitSphere;
            var spwanRotation = anchor.transform.rotation;
            var cardDisplay = CardDisplayBehaviour.Create(card, spawnPos, spwanRotation);

            // add
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
            instanciteCards.Add(new KeyValuePair<CardDisplayBehaviour, DeckTarget>(cardDisplay, belongedDeck));

            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(1f);

        // back to 
        foreach (var record in instanciteCards) {

            BackCardToDeck(record.Key, record.Value);
        }

        yield return new WaitForSeconds(2f);
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
            yield return new WaitForSeconds(0.05f);
        }
    }

    // 各种事件卡的万能入口
    public  IEnumerator ShowEvent(EventCard eventCard, CharacterCard[] bindedCharacters) {
        // show event card selfs
        var eventCardDisplay = ShowCardFromDeck( eventCard, DeckTarget.eventDeck,AnchorManager.instance.eventCardAnchor);

        // show name 
        yield return StartCoroutine(StoryBook.instance.ViewContent(new StoryBook.PageContent(eventCard.name )));
        BackCardToDeck(eventCardDisplay, DeckTarget.eventDeck);

        // show descriptipn
        yield return StartCoroutine(StoryBook.instance.ViewContent(new StoryBook.PageContent(eventCard.description)));

        // show image
        yield return StartCoroutine(StoryBook.instance.ViewContent(new StoryBook.PageContent(eventCard.GetAvatarImage())));

        yield return new WaitForSeconds(1f);
    }

    /// <summary>
    /// 卡片从牌堆抽出来到某个位置的动画
    /// </summary>
    /// <param name="card"></param>
    /// <param name="from"></param>
    /// <param name="anchorPoint"></param>
    /// <returns></returns>
    public CardDisplayBehaviour ShowCardFromDeck(Card card,DeckTarget from, AnchorPoint anchorPoint, bool isAnonymous = false) {
        DeckDisplayBehaviour belongedDeck = GetBlongedDeckDisplay(from);

        Vector3 topPos;
        Quaternion topRotation;
        belongedDeck.DrawCard(out topPos,out topRotation);
        CardDisplayBehaviour cardDisplay;
        if (isAnonymous) {
            cardDisplay = CardDisplayBehaviour.CreateAnonymousCard(topPos, topRotation);
        }
        else {
            cardDisplay = CardDisplayBehaviour.Create(card, topPos, topRotation);
        }

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
