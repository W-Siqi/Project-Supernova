using PCG;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterStatusViewer : MonoBehaviour {
    [SerializeField]
    private List<PersonalityViewerUGUI> personalityViewerUGUIs = new List<PersonalityViewerUGUI>();
    [SerializeField]
    private Image avatarImage;

    [SerializeField]
    private SizeTween loyaltyIconShake;
    [SerializeField]
    private TextMeshProUGUI loyaltyValText;
    [SerializeField]
    private Image loyaltySliderFillImg;
    [SerializeField]
    private TextColorTween loyaltyDiffTextShowAndHide;

    private CharacterCard hookedCharacter = null;
    private int curLoyaltyVal = 0;
    private int initLoyaltyVal = 1;

    public CharacterCard GetHooedCharacter() {
        return hookedCharacter;
    }

    public void HookTo(CharacterCard character) {
        hookedCharacter = character;

        avatarImage.sprite = hookedCharacter.GetAvatarSprite();

        curLoyaltyVal = hookedCharacter.loyalty;
        initLoyaltyVal = hookedCharacter.loyalty;
        loyaltyValText.text = string.Format("{0}/{1}", curLoyaltyVal.ToString(), initLoyaltyVal.ToString());
        loyaltySliderFillImg.fillAmount = (float)curLoyaltyVal / (float)initLoyaltyVal;

        // personality
        if (hookedCharacter.personalities.Length != personalityViewerUGUIs.Count) {
            throw new System.Exception("UI个数不对");
        }
        for (int i = 0; i < hookedCharacter.personalities.Length; i++) {
            personalityViewerUGUIs[i].InitTo(hookedCharacter.personalities[i].trait);
        }
    }

    public void HightlightTrait(Trait trait) {
        foreach (var personalityViewer in personalityViewerUGUIs) {
            if (personalityViewer.currentViewedTrait == trait) {
                personalityViewer.Highlight();
            }
        }
    }
    private void Update() {
        if (hookedCharacter != null) {
            if (curLoyaltyVal != hookedCharacter.loyalty) {
                OnLoyaltyChange(hookedCharacter.loyalty);
            }

            for (int i = 0; i < hookedCharacter.personalities.Length; i++) {
                if (personalityViewerUGUIs[i].currentViewedTrait != hookedCharacter.personalities[i].trait) {
                    personalityViewerUGUIs[i].TransferTo(hookedCharacter.personalities[i].trait);
                }
            }
        }
    }

    private void OnLoyaltyChange(int loyaltyVal) {
        curLoyaltyVal = loyaltyVal;
        loyaltyValText.text = string.Format("{0}/{1}", curLoyaltyVal.ToString(), initLoyaltyVal.ToString());
        loyaltySliderFillImg.fillAmount = (float)curLoyaltyVal / (float)initLoyaltyVal;
        loyaltyIconShake.Play();

        loyaltyDiffTextShowAndHide.Play();
    }
}