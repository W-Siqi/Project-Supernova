using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PCG {
    [System.Serializable]
    public class KeywordConsequence : Consequence {
        public enum Keyword { 
            none,warFight,refreshTraits
        }

        public Keyword keyword = Keyword.none;
    }
}
