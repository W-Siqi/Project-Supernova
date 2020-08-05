using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PCG {
    [System.Serializable]
    public class StratagemCard : Card {
        public string yesText = "可以";
        public string noText = "不行";
        public StratagemConsequenceSet consequenceSet = new StratagemConsequenceSet(); 
    }
}
