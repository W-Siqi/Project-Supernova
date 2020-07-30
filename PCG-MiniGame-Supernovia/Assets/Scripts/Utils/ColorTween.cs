using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorTween : MonoBehaviour
{
    [SerializeField]
    private RuntimeMaterial tweenMat;
    [SerializeField]
    private float playTime = 1f;
    [SerializeField]
    private Color beginColor;
    [SerializeField]
    private Color endColor;
    [SerializeField]
    private AnimationCurve tweenCurve;

    public void Play(){
        StartCoroutine(PlayTween());
    }

    IEnumerator PlayTween() {
        var startTime = Time.time;
        while (Time.time < startTime + playTime) {
            var t = (Time.time - startTime) / playTime;
            tweenMat.runtimeMat.color = Color.Lerp( beginColor,endColor,tweenCurve.Evaluate(t));
            yield return null;
        }
    }
}
