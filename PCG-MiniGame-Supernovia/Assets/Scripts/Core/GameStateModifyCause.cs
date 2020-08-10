using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PCG {
    public class GameStateModifyCause {
        public enum Type { 
            madeStratagemDecision,triggerTrait,eventStream
        }

        public Type type;
        public CharacterCard belongedCharacter;
        public Trait trait;
        public EventCard eventCard;
    }
}
