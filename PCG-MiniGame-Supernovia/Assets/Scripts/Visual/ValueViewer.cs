using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using PCG;

public class ValueViewer : MonoBehaviour
{
    [SerializeField]
    Image activateImg;

    [SerializeField]
    TraitOverlapSign traitOverlapSign;

    [SerializeField]
    private Slider sliderOfValue;  
    [SerializeField]
    private TextMeshProUGUI diffText;

    [SerializeField]
    private SizeTween iconSizeTween;
    [SerializeField]
    private ImageGUIColorTween iconColorTween;

    [SerializeField]
    private TextColorTween diffTextColorTween;
    [SerializeField]
    private Color valueAddDiffTextColor;
    [SerializeField]
    private Color valueMinusDiffTextColor;
    [SerializeField]
    private AudioSource audioWhenChangeValue;

    private float maxVal = 100;
    private float curVal = 0;

    public void ForceSync(int val) {
        curVal  = val;
        sliderOfValue.value = curVal / maxVal;
    }

    public void Activate() {
        activateImg.enabled = true;
    }

    public void Disactivate() {
        activateImg.enabled = false;
    }

    public void ApplyDiff(int delta) {
        curVal += delta;
        sliderOfValue.value = curVal / maxVal;
        ShowDiff(delta);
    }

    public void ApplyDiff(int delta,Trait showTrait) {
        traitOverlapSign.ShowSign(showTrait);
        ApplyDiff(delta);
    }

    void ShowDiff(float diffValue) {     
        if (diffValue > 0) {
            iconColorTween.endColor = valueAddDiffTextColor;
            diffTextColorTween.beginColor = diffTextColorTween.endColor = valueAddDiffTextColor;
            diffText.text = "+" + diffValue.ToString();
        }
        else {
            iconColorTween.endColor = valueMinusDiffTextColor;
            diffTextColorTween.beginColor = diffTextColorTween.endColor = valueMinusDiffTextColor;
            diffText.text = diffValue.ToString();
        }

        diffTextColorTween.beginColor.a = 0;
        diffTextColorTween.endColor.a = 1;
        diffTextColorTween.Play();

        iconColorTween.Play();
        iconSizeTween.Play();

        audioWhenChangeValue.Play();
    }
}
