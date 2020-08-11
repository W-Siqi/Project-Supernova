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

        public float tolerantTraitKeepLoyaltyProbability = 0.5f;
        public float tolerantWipeCruelProb = 0.5f;

        public float honestProbabilityPerStratagem = 0.5f;

        public int jealousTraitThreshold = 3;

        public int corrputTraitMoneyPerRound = 10;
        public int corrputTraitMoneyPerStratagem = 5;
        public float corrputTraitStratagemProb = 0.4f;

        public float trickyTraitTriggerProb = 0.6f;

        public int cruelTraitPeopleValuePerDecision = 5;

        public int warlikeTraitArmyValueWhenAccept = 10;

        public GameConfig MakeDeepCopy() {
            return JsonUtility.FromJson<GameConfig>(JsonUtility.ToJson(this));
        }
    }
}