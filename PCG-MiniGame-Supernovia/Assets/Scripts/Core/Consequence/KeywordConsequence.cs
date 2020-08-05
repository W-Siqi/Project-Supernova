using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PCG {
    [System.Serializable]
    public class KeywordConsequence : Consequence {
        public enum Keyword { 
            none,warFight
        }

        public Keyword keyword = Keyword.none;
    }
}
