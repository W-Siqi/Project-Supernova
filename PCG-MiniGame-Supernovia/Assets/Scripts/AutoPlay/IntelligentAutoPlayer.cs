using PCG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntelligentAutoPlayer:AutoPlayer
{
    protected override float ProbailityOfAccept(StratagemCard stratagemCard, CharacterCard provider, GameState gameState) {
        if (provider.loyalty == 1) { 
            return 1.0f;
        }
        return 0.5f;
    }
}
