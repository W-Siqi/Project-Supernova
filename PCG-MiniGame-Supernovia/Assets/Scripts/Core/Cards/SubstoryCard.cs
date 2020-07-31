using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PCG {
    [System.Serializable]
    public class SubstoryCard : Card {
        /// <summary>
        /// dungeon,副本卡 ； sideQuest， 支线卡
        /// </summary>
        public enum Type {
            dungeon, sideQuest
        }

        public Type type = Type.dungeon;
        // 副本专属的角色
        public List<CharacterCard> newCharacters = new List<CharacterCard>();
        public List<StratagemCard> stratagemCards = new List<StratagemCard>();
        public List<EventCard> eventCards = new List<EventCard>();
    }
}
