using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizeTween : MonoBehaviour
{
    [SerializeField]
    private float playTime = 1f;
    [SerializeField]
    private AnimationCurve sizeCurve;

    [ContextMenu("TEST Play")]
    public void Play() {
        StartCoroutine(PlayTween());
    }

    IEnumerator PlayTween() {
        var originalSize = transform.localScale;
        var startTime = Time.time;
        while (Time.time < startTime + playTime) {
            var t = (Time.time - startTime) / playTime;
            transform.localScale = originalSize * sizeCurve.Evaluate(t);
            yield return null;
        }
        transform.localScale = originalSize;
    }
}
