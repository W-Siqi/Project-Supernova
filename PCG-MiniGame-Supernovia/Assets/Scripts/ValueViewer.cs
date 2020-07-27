using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ValueViewer : MonoBehaviour
{
    [SerializeField]
    public Slider sliderOfValue;

    private float maxVal = 100;
    private float curVal = 100;

    public void SetMaxValue(float maxVal) {
        this.maxVal = maxVal;
    }

    public void SetCurrentValue(float curVal) {
        this.curVal = Mathf.Clamp(curVal, 0, maxVal);
        sliderOfValue.value = this.curVal / maxVal;
    }
}
