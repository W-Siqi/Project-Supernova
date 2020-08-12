using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PCG {
    [System.Serializable]
    public class GameLogPackage : ScriptableObject {
        public GameState gameState;
        public MultiAutoPlayStatistic summerayStatistic = new MultiAutoPlayStatistic();
        public List<SingleAutoPlayStatistic> playStatistics= new List<SingleAutoPlayStatistic>();
    }
}
