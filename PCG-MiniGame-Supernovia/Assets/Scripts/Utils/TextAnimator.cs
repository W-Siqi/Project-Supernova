﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace PCG {
    public class TextAnimator : MonoBehaviour {
        [SerializeField]
        private TextMeshProUGUI textMeshPro;
        [SerializeField]
        private float characterTimeInterval = 0.06f;

        private int animID = 0;

        public void Play(string content,float delay = 0) {
            textMeshPro.text = "";
            StartCoroutine(PlayAnimationWithID(++animID, content, delay));
        }

        public void Clear() {
            animID++;
        }

        IEnumerator PlayAnimationWithID(int assignedID, string content, float delay) {
            yield return new WaitForSeconds(delay);

            string curStr = "";
            foreach (var c in content) {
                if (animID != assignedID) {
                    break;
                }
                curStr += c;
                textMeshPro.text = curStr;
                yield return new WaitForSeconds(characterTimeInterval);
            }
        }
    }
}
