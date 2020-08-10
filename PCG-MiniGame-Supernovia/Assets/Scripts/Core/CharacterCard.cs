using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace PCG {
    [System.Serializable]
    public class CharacterCard : Card {
        public const int PERSONALITY_COUNT = 3;
        public int loyalty = 3;

        [System.Serializable]
        public class Attributes {
            public int atkVal = 0;
            public int maxHP = 0;
            public int HP = 0;
        }

        public Attributes attributes = new Attributes();
        public Personality[] personalities = new Personality[PERSONALITY_COUNT];

        public bool HasTrait(Trait trait) {
            foreach (var p in personalities) {
                if (p.trait == trait) {
                    return true;
                }
            }
            return false;
        }

        public int FindPersonaltyIndex(Personality personality) {
            for(int i = 0; i < personalities.Length; i++){
                if (personalities[i] == personality) {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// 返回-1如果没有的
        /// </summary>
        /// <param name="trait"></param>
        /// <returns></returns>
        public int FindPersonaltyIndex(Trait trait) {
            for (int i = 0; i < personalities.Length; i++) {
                if (personalities[i].trait == trait) {
                    return i;
                }
            }
            return -1;
        }
    }
}
