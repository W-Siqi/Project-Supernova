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
    private TextColorTween diffTextColorTween;
    [SerializeField]
    private Color valueAddDiffTextColor;
    [SerializeField]
    private Color valueMinusDiffTextColor;

    private float maxVal = 100;
    private float curVal = 100;

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

        this.curVal = Mathf.Clamp(curVal, 0, maxVal);
        sliderOfValue.value = this.curVal / maxVal;
    }

    private void Awake() {
        SetCurrentValue(curVal);
    }

    void ShowDiff(float diffValue) {
        iconSizeTween.Play();
        if (diffValue > 0) {
            diffTextColorTween.beginColor = diffTextColorTween.endColor = valueAddDiffTextColor;
            diffText.text = "+" + diffValue.ToString();
        }
        else {
            diffTextColorTween.beginColor = diffTextColorTween.endColor = valueMinusDiffTextColor;
            diffText.text = diffValue.ToString();
        }
        diffTextColorTween.beginColor.a = 0;
        diffTextColorTween.endColor.a = 1;
        diffTextColorTween.Play();
    }
}
