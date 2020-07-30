using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ValueViewer : MonoBehaviour
{
    [SerializeField]
    public Slider sliderOfValue;
    [SerializeField]
    private Text diffText;

    private float maxVal = 100;
    private float curVal = 100;

    public void SetMaxValue(float maxVal) {
        this.maxVal = maxVal;
    }

    public void SetCurrentValue(float curVal) {
        var diff = curVal - this.curVal;

        this.curVal = Mathf.Clamp(curVal, 0, maxVal);
        sliderOfValue.value = this.curVal / maxVal;
    }

    void ShowDiff(float diffValue) {
        if (diffValue > 0) {
            diffText.text = "+" + diffValue.ToString();
        }
        else {
            diffText.text = "-" + diffValue.ToString();
        }
    }
}
