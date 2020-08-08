using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationTween : MonoBehaviour
{
    public bool autoPingPongLoop = false;
    public Transform startAnchor;
    public Transform endAnchor;
    public AnimationCurve mappingCurve;
    public float duration = 1f;

    private void Awake() {
        transform.rotation = startAnchor.rotation;
        if (autoPingPongLoop) {
            StartCoroutine(PlayTweenPingPongLoop());
        }
    }

    [ContextMenu("Play Tween")]
    private void TestInInspector() {
        Play(false);
    }

    public void Play(bool reverse = false) {
        Quaternion from;
        Quaternion to;
        if (reverse) {
            from = endAnchor.rotation;
            to = startAnchor.rotation;
        }
        else {
            from = startAnchor.rotation;
            to = endAnchor.rotation;
        }
        LerpAnimator.instance.LerpValues(0, 1, duration,
            (v) => {
                var t = mappingCurve.Evaluate(v);
                transform.rotation =Quaternion.Lerp(from, to, t);
            });
    }

    IEnumerator PlayTweenPingPongLoop() {
        while (true) {
            Play();
            yield return new WaitForSeconds(duration);
            Play(true);
            yield return new WaitForSeconds(duration);
        }
    }
    public void ResetToStart() {
        transform.position = startAnchor.position;
    }
}
