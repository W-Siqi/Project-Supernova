using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace PCG {
    [System.Serializable]
    public class CharacterCard : Card {
        public const int PERSONALITY_COUNT = 3;

        [System.Serializable]
        public class Attributes {
            public int atkVal = 0;
            public int maxHP = 0;
            public int HP = 0;
        }

        public Attributes attributes = new Attributes();
        public Personality[] personalities = new Personality[PERSONALITY_COUNT];
    }
}
