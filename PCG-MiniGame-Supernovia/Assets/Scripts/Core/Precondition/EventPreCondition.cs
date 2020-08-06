﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PCG {
    [System.Serializable]
    public class EventPrecondition : Precondition {
        public string eventCardName = "";

        public EventPrecondition(EventCard eventCard) {
            eventCardName = eventCard.name;
        }

        public override bool SatisfiedAt(GameState givenState) {
            throw new System.NotImplementedException();
        }
    }
}
