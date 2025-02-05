﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizeTween : MonoBehaviour
{
    public float playTime = 1f;

    [SerializeField]
    private AnimationCurve sizeCurve;
    [SerializeField]
    private bool resetAfterTweenOver = false;

    int playID = 0;
    private Vector3 originalSize;

    private void Awake() {
        originalSize = transform.localScale;
    }

    [ContextMenu("TEST Play")]
    public void Play() {
        StartCoroutine(PlayTween(++playID));
    }

    IEnumerator PlayTween(int assignedID) {
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
