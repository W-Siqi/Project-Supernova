using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class ValueViewer : MonoBehaviour
{
    [Range(0,100)]
    public float debugValue = 60;

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

    [ContextMenu("Test with debug value")]
    public void DebugTest() {
        SetCurrentValue(debugValue);
    }

    public void SetMaxValue(float maxVal) {
        this.maxVal = maxVal;
    }

    public void SetCurrentValue(float curVal) {
        var diff = curVal - this.curVal;
        if (Mathf.Abs( diff) > 0.01) {
            ShowDiff(diff);
        }

        this.curVal = curVal;
        sliderOfValue.value = Mathf.Clamp(this.curVal, 0, maxVal) / maxVal;
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
