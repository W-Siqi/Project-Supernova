using PCG;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StoryEndingManager : MonoBehaviour
{
    [SerializeField]
    private Text endingTitle;
    [SerializeField]
    private Text endingContent;
    [SerializeField]
    private RawImage endingImage;

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

    public void  OnStoryEnd() {
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

        // fill content
        if (win) {
            endingTitle.text = "成功结局";
        }
        else {
            endingTitle.text = "失败结局";
        }

        endingContent.text = pageContentOfEnd.text;
        endingImage.texture = pageContentOfEnd.image;

        ViewManager.instance.OnStortEnd();
    }

    public void Restart() {
        SceneManager.LoadScene("Main");
    }
}
