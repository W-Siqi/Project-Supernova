using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PCG {
    [System.Serializable]
    public class Personality {
        public Trait currentTrait;

        // TBD
        /// <summary>
        /// 返回0 表示当前personality 就是这个状态； 
        /// 如果整个graph没有这个节点，返回无限大
        /// </summary>
        /// <param name="qualifier"></param>
        /// <returns></returns>
        public int TopologyDistanceFromCurrent(Trait trait) {
            return 0;
        }

        // TBD
        public void StateTransfer(Trait from, Trait to, int fromSearchDistance = 0, int toSearchDistance= 0) {
            currentTrait = to;
        }
    }
}