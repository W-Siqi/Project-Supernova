using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PCG {
    [System.Serializable]
    public class StratagemCard : Card {
        public PreconditonSet preconditonSet = new PreconditonSet();
        public ConsequenceSet consequenceSet = new ConsequenceSet();
    }
}
