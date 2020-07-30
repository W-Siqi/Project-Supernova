using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PCG {
    [System.Serializable]
    public class EventCard : Card {
        /// <summary>
        /// 匿名表示，具体人物在演示的时候，不会让玩家知道具体是谁
        /// </summary>
        public bool isAanonymous = false;
        public string discruption = "this is a event, somthing happened";
        public PreconditonSet preconditonSet = new PreconditonSet();
        public ConsequenceSet consequenceSet = new ConsequenceSet();
    }
}
