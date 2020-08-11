using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PCG {
    public class GlobalSettings : MonoBehaviour {
        public static GlobalSettings instance;

        public bool openTutorial;
        public float savedDifficulty = 0.3f;
        public bool theSingleton = false;

        private void Awake() {
            foreach (var gs in FindObjectsOfType<GlobalSettings>()) {
                if (gs.theSingleton) {
                    DestroyImmediate(gameObject);
                    return;
                }
            }

            instance = this;
            DontDestroyOnLoad(this);
        }
    }
}