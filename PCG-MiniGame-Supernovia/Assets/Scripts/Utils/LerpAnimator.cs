﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpAnimator : MonoBehaviour {
    public delegate void OnLerpDone();
    public delegate void OnNewLerpValue(float value);

    private static LerpAnimator _instance = null;
    public static LerpAnimator instance {
        get {
            if (_instance == null) {
                var GO = new GameObject("LerpAnimator");
                _instance = GO.AddComponent<LerpAnimator>();
            }
            return _instance;
        }
    }

    public void LerpValues(float from,float to, float playTime, OnNewLerpValue onNewLerpValue) {
        StartCoroutine(PlayValueAnimation(from,to,playTime,onNewLerpValue));
    }

    /// <summary>
    /// animation curve 的 y就是实际的val,代表幅度。 x是0~1代表时间进程
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <param name="playTime"></param>
    /// <param name="animationCurve"></param>
    /// <param name="onNewLerpValue"></param>
    public void LerpValues(AnimationCurve animationCurve, float playTime,  OnNewLerpValue onNewLerpValue) {
        StartCoroutine(PlayValueAnimation(animationCurve, playTime, onNewLerpValue));
    }

    IEnumerator PlayValueAnimation(float from, float to, float playTime, OnNewLerpValue onNewLerpValue) {
        var startTime = Time.time;
        while (Time.time < startTime + playTime) {
            var t = (Time.time - startTime) / playTime;
            onNewLerpValue(Mathf.Lerp(from,to,t));
            yield return null;
        }
        onNewLerpValue(to);
    }

    IEnumerator PlayValueAnimation(AnimationCurve animationCurve, float playTime, OnNewLerpValue onNewLerpValue) {
        var startTime = Time.time;
        while (Time.time < startTime + playTime) {
            var t = (Time.time - startTime) / playTime;
            var val = animationCurve.Evaluate(t);
            
            onNewLerpValue(val);
            yield return null;
        }
    }

    /// <summary>
    /// 以当前状态为起点
    /// </summary>
    /// <param name="target"></param>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <param name="playTime"></param>
    /// <param name="onLerpDone"></param>
    public void LerpPositionAndRotation(Transform target, Vector3 destPosition,Quaternion destRotaton, float playTime, OnLerpDone onLerpDone = null) {
        StartCoroutine(LerpTransformAnimation(target,
            target.position, destPosition,
            target.rotation, destRotaton,
            target.localScale, target.localScale,
            playTime, onLerpDone));
    }

    /// <summary>
    /// 以当前状态为起点
    /// </summary>
    /// <param name="target"></param>
    /// <param name="destPosition"></param>
    /// <param name="destRotaton"></param>
    /// <param name="playTime"></param>
    /// <param name="onLerpDone"></param>
    public void LerpPosition(Transform target, Vector3 destPosition, float playTime, OnLerpDone onLerpDone = null) {
        StartCoroutine(LerpTransformAnimation(target,
            target.position, destPosition,
            target.rotation, target.rotation,
            target.localScale, target.localScale,
            playTime, onLerpDone));
    }

    IEnumerator LerpTransformAnimation(Transform target,
        Vector3 fromPos, Vector3 toPos,
        Quaternion fromRotation, Quaternion toRotation,
        Vector3 fromScale, Vector3 toScale,
        float playTime,OnLerpDone onLerpDone) {

        var startTime = Time.time;
        while (Time.time < startTime+playTime) {
            var t = (Time.time - startTime) / playTime;
            target.transform.position = Vector3.Lerp(fromPos, toPos, t);
            target.transform.rotation = Quaternion.Lerp(fromRotation, toRotation, t);
            target.transform.localScale = Vector3.Lerp(fromScale, toScale, t);
            yield return null;
        }
        target.transform.position = toPos;
        target.transform.rotation = toRotation;
        target.transform.localScale = toScale;

        if (onLerpDone != null) {
            onLerpDone();
        }
    }
}
