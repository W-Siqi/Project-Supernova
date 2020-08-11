using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PCG {
    [System.Serializable]
    public class DiscreteQuantifyValue {
        public int from = 1;
        public int to = 10;
        public int step = 1;
        public List<double> difficultyFactors;
    }
}
