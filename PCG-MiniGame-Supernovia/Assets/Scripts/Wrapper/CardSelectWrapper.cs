using PCG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardSelectWrapper : MonoBehaviour
{
    public CardDeckInterface cardDeck;
    public float selectedSizeAmplifer = 3f;
    private CharacterCard[] characterCards;

    public void Init(CharacterCard[] characterCards) {
        this.characterCards = characterCards;
        cardDeck.MannualStart(characterCards);
    }

    void Start() {
        /*
         * You can use OnCardSelected callback 
         * if you prefer to wait for the animation finished
         * or the way in the Update() function to get the index right after the user click the card
         */
        cardDeck.OnCardSelected = OnCardChanged;
    }

    void OnCardChanged(int cardIndex) {
        CardSelect.Card currentCard = cardDeck.GetCurrentCard();
        for (int i = 0; i < cardDeck.Size(); i++) {
            if (cardIndex == i) {
                cardDeck.cards[i].transform.localScale = Vector3.one * selectedSizeAmplifer;
            }
            else {
                cardDeck.cards[i].transform.localScale = Vector3.one;
            }
        }
    }

    // Update is called once per frame
    void Update() {

        /*
         * You can use OnCardSelected callback 
         * if you prefer to wait for the animation finished
         * or the way below to get the index right after the user is click the card
         */
        //Card currentCard = cardDeck.GetCurrentCard();

        //if (currentCard != null)
        //    selectedCardText.text = sampleDescription + currentCard.tooltipMessage;

        if (Input.GetKeyUp(KeyCode.LeftArrow)) {
            cardDeck.SetCurrentIndex(cardDeck.GetCurrentIndex() - 1);
        }

        if (Input.GetKeyUp(KeyCode.RightArrow)) {
            cardDeck.SetCurrentIndex(cardDeck.GetCurrentIndex() + 1);
        }
    }
}
