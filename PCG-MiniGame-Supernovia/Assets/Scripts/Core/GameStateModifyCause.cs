using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PCG {
    [System.Serializable]
    public class GameStateModifyCause {
        public enum Type { 
            madeStratagemDecision,triggerTrait,eventStream
        }

        public Type type;
        public int belongedCharacterIndex;
        public Trait trait;
        public int eventCardIndex;
    }
}
