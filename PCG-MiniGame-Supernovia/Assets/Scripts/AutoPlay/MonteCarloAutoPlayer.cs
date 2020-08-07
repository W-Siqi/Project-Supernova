using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PCG {
    public class MonteCarloAutoPlayer : AutoPlayer {
        protected override float ProbailityOfAccept(StratagemCard stratagemCard, CharacterCard provider, GameState gameState) {
            return 0.5f;
        }
    }
}