using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PCG {
    // 下标要结合给定的gamestate才有效
    [System.Serializable]
    public class BindingInfo {
        public int bindedCharacterIndex = 0;
        public int bindedPersonalityIndex = 0;

        public CharacterCard GetBindedCharacer(GameState gameState) {
            return gameState.characterDeck[bindedCharacterIndex];
        }

        public Personality GetBindedPersonality(GameState gameState) {
            return gameState.characterDeck[bindedCharacterIndex].personalities[bindedPersonalityIndex];
        }
    }
}
