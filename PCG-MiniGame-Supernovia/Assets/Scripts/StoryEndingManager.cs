using PCG;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StoryEndingManager : MonoBehaviour
{
    public Text endingTitle;
    public Text endingContent;
    public RawImage endingImage;

    public StoryBook.PageContent goodEnd;
    public StoryBook.PageContent badEndMoney;
    public StoryBook.PageContent badEndArmy;
    public StoryBook.PageContent badEndPeople;
    public StoryBook.PageContent badEndLoyalty;

    public void  OnStoryEnd(GameState gameState) {
        var status =gameState.statusVector;
        StoryBook.PageContent pageContentOfEnd = null;
        bool win = false;

        CharacterCard zeroLoyaltyCharacter = null;
        foreach (var character in gameState.characterDeck) {
            if (character.loyalty <= 0) {
                zeroLoyaltyCharacter = character;
                break;
            }
        }

        endingContent.text = "";
        if (zeroLoyaltyCharacter != null) {
            endingContent.text += string.Format("【{0}】的忠诚度降为零\n", zeroLoyaltyCharacter.name);
            pageContentOfEnd = badEndLoyalty;
            ViewManager.instance.CharacterBetralEnding(zeroLoyaltyCharacter);
        }
        else if (status.army <= 0) {
            endingContent.text += string.Format("你的【军队】值 降为零\n");
            pageContentOfEnd = badEndArmy;
        }
        else if (status.money <= 0) {
            endingContent.text += string.Format("你的【财政】值 降为零0\n");
            pageContentOfEnd = badEndMoney;
        }
        else if (status.people <= 0) {
            endingContent.text += string.Format("你的【民心】值 降为零\n");
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

        endingContent.text += pageContentOfEnd.text;
        endingImage.texture = pageContentOfEnd.image;

        ViewManager.instance.OnStortEnd();
    }

    public void Restart() {
        SceneManager.LoadScene("Main");
    }
}
