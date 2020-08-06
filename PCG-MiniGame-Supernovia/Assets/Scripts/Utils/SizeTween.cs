using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizeTween : MonoBehaviour
{
    [SerializeField]
    private float playTime = 1f;
    [SerializeField]
    private AnimationCurve sizeCurve;
    [SerializeField]
    private bool resetAfterTweenOver = false;

    int playID = 0;
    [ContextMenu("TEST Play")]
    public void Play() {
        StartCoroutine(PlayTween(++playID));
    }

    IEnumerator PlayTween(int assignedID) {
        var originalSize = transform.localScale;
        var startTime = Time.time;
        while (assignedID == playID && Time.time < startTime + playTime) {
            var t = (Time.time - startTime) / playTime;
            transform.localScale = originalSize * sizeCurve.Evaluate(t);
            yield return null;
        }

        if (assignedID == playID && resetAfterTweenOver) {
            transform.localScale = originalSize;
        }
    }
}
