using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpAnimator : MonoBehaviour
{
    public delegate void OnLerpDone();

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
