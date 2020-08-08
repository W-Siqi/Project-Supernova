using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionTween : MonoBehaviour
{
    public bool autoPingPongLoop = false;
    public Transform startAnchor;
    public Transform endAnchor;
    public AnimationCurve mappingCurve;
    public float duration = 1f;

    private void Awake() {
        transform.position = startAnchor.position;
        if (autoPingPongLoop) {
            StartCoroutine(PlayTweenPingPongLoop());
        }
    }

    [ContextMenu("Play Tween")]
    private void TestInInspector() {
        Play(false);
    }

    public void Play(bool reverse = false) {
        Vector3 from;
        Vector3 to;
        if (reverse) {
            from = endAnchor.position;
            to = startAnchor.position;
        }
        else {
            from = startAnchor.position;
            to = endAnchor.position;
        }
        LerpAnimator.instance.LerpValues(0, 1, duration,
            (v) => {
                var t = mappingCurve.Evaluate(v);
                transform.position = Vector3.Lerp(from, to, t);
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
