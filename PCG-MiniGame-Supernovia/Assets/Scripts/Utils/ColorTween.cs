using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PCG {
    public abstract class ColorTween : MonoBehaviour {
        public Color beginColor;
        public Color endColor;
        [SerializeField]
        private float playTime = 1f;
        [SerializeField]
        private AnimationCurve tweenCurve;

        [ContextMenu("Test Play")]
        public void Play() {
            StartCoroutine(PlayTween());
        }

        protected abstract void SetColor(Color color);

        IEnumerator PlayTween() {
            var startTime = Time.time;
            while (Time.time < startTime + playTime) {
                var t = (Time.time - startTime) / playTime;
                SetColor(Color.Lerp(beginColor, endColor, tweenCurve.Evaluate(t)));
                yield return null;
            }
        }
    }
}