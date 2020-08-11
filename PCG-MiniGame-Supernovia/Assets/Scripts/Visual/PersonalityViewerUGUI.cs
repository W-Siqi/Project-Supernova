using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PCG;
using TMPro;
using UnityEngine.UI;

public class PersonalityViewerUGUI : MonoBehaviour
{
    private const string DISSOLVE_SHADER_PROPERTY = "_SliceAmount";
    public Trait currentViewedTrait { get; private set; } = Trait.none;
    [SerializeField]
    TraitTooltipDetector tooltipDetector;
    [SerializeField]
    SizeTween hightlightSizeTween;
    [SerializeField]
    ImageGUIColorTween hightlightColorTween;
    [SerializeField]
    Image background;
    [SerializeField]
    TextMeshProUGUI traitName;
    [SerializeField]
    Image activateImg;
    private int hightlightAniID = 0;
    public void InitTo(Trait trait) {
        currentViewedTrait = trait;
        traitName.text = TraitUtils.TranslateToName(trait);
        background.color = GetTraitColor(trait);
    }

    public void TransferTo(Trait trait) {
        currentViewedTrait = trait;
        traitName.text = TraitUtils.TranslateToName(trait);
        background.color = GetTraitColor(trait);
        HighlightOn(3f);
    }

    public void Activate(string toolTip) {
        activateImg.enabled = true;
        tooltipDetector.toolTip = toolTip;
    }

    public void Disactivate() {
        activateImg.enabled = false;
    }

    public void HighlightOn(float duration = 4f) {
        StartCoroutine(HightlightLooping(duration, ++hightlightAniID));
    }

    public void HighlightClose() {
        hightlightAniID++;
    }

    private IEnumerator HightlightLooping(float duration, int id) {
        float freq = 2f;
        hightlightColorTween.playTime = freq;
        hightlightSizeTween.playTime = freq;
        var playedTime = 0f;
        while (playedTime < duration && id == hightlightAniID) {
            hightlightColorTween.Play();
            hightlightSizeTween.Play();
            playedTime += freq;
            yield return new WaitForSeconds(freq);
        }
    }

    private Color GetTraitColor(Trait trait) {
        if (TraitUtils.IsEvil(trait)) {
            return ViewManager.instance.resTable.evilTraitColor;
        }
        else {
            return ViewManager.instance.resTable.noneEvilTraitColor;
        }
    }
}
