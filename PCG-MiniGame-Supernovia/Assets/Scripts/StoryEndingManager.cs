using PCG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryEndingManager : MonoBehaviour
{
    [SerializeField]
    private StoryBook.PageContent goodEnd;
    [SerializeField]
    private StoryBook.PageContent badEndMoney;
    [SerializeField]
    private StoryBook.PageContent badEndArmy;
    [SerializeField]
    private StoryBook.PageContent badEndPeople;
    [SerializeField]
    private StoryBook.PageContent badEndLoyalty;
    public IEnumerator PlayStoryEnding() {
        var status =PlayData.instance.gameState.statusVector;
        StoryBook.PageContent pageContentOfEnd = null;
        bool win = false;

        CharacterCard zeroLoyaltyCharacter = null;
        foreach (var character in PlayData.instance.gameState.characterDeck) {
            if (character.loyalty <= 0) {
                zeroLoyaltyCharacter = character;
                break;
            }
        }

        if (zeroLoyaltyCharacter != null) {
            pageContentOfEnd = badEndLoyalty;
        }
        else if (status.army <= 0) {
            pageContentOfEnd = badEndArmy;
        }
        else if (status.money <= 0) {
            pageContentOfEnd = badEndMoney;
        }
        else if (status.people <= 0) {
            pageContentOfEnd = badEndPeople;
        }
        else {
            win = true;
            pageContentOfEnd = goodEnd;
        }

        yield return StartCoroutine(PlayEndingPage(pageContentOfEnd));
    }

    private IEnumerator PlayEndingPage(StoryBook.PageContent content) {
        StoryBook.instance.ViewContent(content);
        yield return null;
    }
}
