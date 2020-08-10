using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PCG {
    [System.Serializable]
    public class GameState {
        public List<CharacterCard> characterDeck = new List<CharacterCard>();
        public List<EventCard> eventDeck = new List<EventCard>();
        public Dictionary<CharacterCard, List<StratagemCard>> stratagemDict = new Dictionary<CharacterCard, List<StratagemCard>>();
        public StatusVector statusVector = new StatusVector();
        public int acceptCountInCurrentRound = 0;

        public GameState MakeDeepCopy() {
            var gs = new GameState();
            foreach (var c in characterDeck) {
                var copiedCard = Card.DeepCopy(c);
                gs.characterDeck.Add(copiedCard);

                gs.stratagemDict[copiedCard] = new List<StratagemCard>();
                foreach (var stra in stratagemDict[c]) {
                    gs.stratagemDict[copiedCard].Add(Card.DeepCopy(stra));
                }
            }

            foreach (var c in eventDeck) {
                gs.eventDeck.Add(Card.DeepCopy(c));
            }

            gs.statusVector = new StatusVector(statusVector);

            return gs;
        }

        /// <summary>
        /// 主要是还原人物的状态，和国家状态； 
        /// 其他的都不变
        /// </summary>
        /// <param name="src"></param>
        public void RecoverTo(GameState src) {
            for (int i = 0; i < characterDeck.Count; i++) {
                characterDeck[i].loyalty = src.characterDeck[i].loyalty;
                for (int j = 0; j < CharacterCard.PERSONALITY_COUNT; j++) {
                    characterDeck[i].personalities[j].trait = src.characterDeck[i].personalities[j].trait;
                }
            }

            statusVector.army = src.statusVector.army;
            statusVector.money = src.statusVector.money;
            statusVector.people = src.statusVector.people;
        }

        public void RefreshForNewRound() {
            acceptCountInCurrentRound = 0;
        }

        public int GetIndex(CharacterCard character) {
            for (int i = 0; i < characterDeck.Count; i++) {
                if (characterDeck[i] == character) {
                    return i;
                }
            }
            return -1;
        }
    }
}
