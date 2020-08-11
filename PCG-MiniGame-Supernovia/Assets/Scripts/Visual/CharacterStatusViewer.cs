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
    private TraitOverlapSign traitOverlapSign;

    [SerializeField]
    private SizeTween loyaltyIconShake;
    [SerializeField]
    private TextMeshProUGUI loyaltyValText;
    [SerializeField]
    private Image loyaltySliderFillImg;
    [SerializeField]
    private TextColorTween loyaltyDiffTextShowAndHide;
    [SerializeField]
    private TextMeshProUGUI loyaltyChangeDiffText;

    [SerializeField]
    private SizeTween sentanceShowupAndHide;
    [SerializeField]
    private Text sentanceText;

    private CharacterCard hookedCharacter = null;
    private int curLoyaltyVal = 0;
    private int initLoyaltyVal = 1;

    public void ForceSync() {
        curLoyaltyVal = hookedCharacter.loyalty;
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

    public void ActivateTrait(Trait trait,string tooltip) {
        foreach (var personalityViewer in personalityViewerUGUIs) {
            if (personalityViewer.currentViewedTrait == trait) {
                personalityViewer.Activate(tooltip);
            }
        }
    }

    public void DisactivateAllTraits() {
        foreach (var personalityViewer in personalityViewerUGUIs) {
            personalityViewer.Disactivate();
        }
    }

    public void HightlightTrait(Trait trait) {
        foreach (var personalityViewer in personalityViewerUGUIs) {
            if (personalityViewer.currentViewedTrait == trait) {
                personalityViewer.HighlightOn();
            }
        }
    }

    public IEnumerator ViewTraitChange(int personaltyIndex, Trait newTrait) {
        float waitTime = 2f;
        personalityViewerUGUIs[personaltyIndex].TransferTo(newTrait);
        traitOverlapSign.ShowSign(newTrait);
        yield return new WaitForSeconds(waitTime);
    }

    public IEnumerator ViewLoyaltyDelta(int loyaltyDelta) {
        curLoyaltyVal += loyaltyDelta;
        loyaltyValText.text = string.Format("{0}/{1}", curLoyaltyVal.ToString(), initLoyaltyVal.ToString());
        loyaltySliderFillImg.fillAmount = (float)curLoyaltyVal / (float)initLoyaltyVal;
        loyaltyIconShake.Play();
        loyaltyDiffTextShowAndHide.Play();

        if (loyaltyDelta < 0) {
            loyaltyChangeDiffText.text = string.Format("忠诚度 {0}", loyaltyDelta);
            yield return StartCoroutine(ViewCharacterSentance("陛下真让人失望"));
        }
        else if (loyaltyDelta == 0) {
            loyaltyChangeDiffText.text = string.Format("忠诚度不变", loyaltyDelta);
            yield return StartCoroutine(ViewCharacterSentance("陛下不采纳也罢"));
        }
        else {
            yield return StartCoroutine(ViewCharacterSentance("我对陛下忠心耿耿"));
        }
    }

    // 后面的对话会自动覆盖前面的，不用担心冲突
    public IEnumerator ViewCharacterSentance(string sentance) {
        sentanceShowupAndHide.gameObject.SetActive(true);
        float duration = 3f;
        float waitTime = 2f;
        sentanceShowupAndHide.playTime = duration;
        sentanceShowupAndHide.Play();
        sentanceText.text = sentance;
        yield return new WaitForSeconds(waitTime);
    }
}