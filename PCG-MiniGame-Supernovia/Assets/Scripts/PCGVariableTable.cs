using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PCG {
    [System.Serializable]
    public class GameConfig {
        public int roundCount = 10;
        public int characterCount = 4;
        public int eventCountPerRound = 2;

        public float wiseTraitAmplifyRate = 0.3f;
        public float slicentTraitSlicenceProbility = 0.3f;
        public int corrputTraitMoneyPerRound = 2;
        public int cruelTraitPeopleValuePerDecision = 5;

        public GameConfig MakeDeepCopy() {
            return JsonUtility.FromJson<GameConfig>(JsonUtility.ToJson(this));
        }
    }
}