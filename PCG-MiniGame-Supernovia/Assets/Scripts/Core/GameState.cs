using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PCG {
    [System.Serializable]
    public class GameState {
        public List<CharacterCard> characterDeck = new List<CharacterCard>();
        public List<EventCard> eventDeck = new List<EventCard>();
        public List<StratagemCard> stratagemDeck = new List<StratagemCard>();
        public Dictionary<CharacterCard, List<int>> stratagemIndexDict = new Dictionary<CharacterCard, List<int>>();
        public StatusVector statusVector = new StatusVector();
        public int acceptCountInCurrentRound = 0;

        public GameState MakeDeepCopy() {
            var gs = new GameState();

            foreach (var s in stratagemDeck) {
                gs.stratagemDeck.Add(Card.DeepCopy(s));
            }

            foreach (var c in eventDeck) {
                gs.eventDeck.Add(Card.DeepCopy(c));
            }

            foreach (var c in characterDeck) {
                var copiedCard = Card.DeepCopy(c);
                gs.characterDeck.Add(copiedCard);

                // 复制下标序列
                gs.stratagemIndexDict[copiedCard] = new List<int>();
                foreach (var straIndex in stratagemIndexDict[c]) {
                    gs.stratagemIndexDict[copiedCard].Add(straIndex);
                }
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

        public StratagemCard GetStratagem(CharacterCard characterCard, int round) {
            var indexSequence = stratagemIndexDict[characterCard];
            return stratagemDeck[indexSequence[round]];
        }

        public int GetIndex(CharacterCard character) {
            for (int i = 0; i < characterDeck.Count; i++) {
                if (characterDeck[i] == character) {
                    return i;
                }
            }
            return -1;
        }

        public int GetIndex(EventCard eventCard) {
            for (int i = 0; i < eventDeck.Count; i++) {
                if (eventDeck[i] == eventCard) {
                    return i;
                }
            }
            return -1;
        }

        public int GetIndex(StratagemCard stratagemCard) {
            for (int i = 0; i < stratagemDeck.Count; i++) {
                if (stratagemDeck[i] == stratagemCard) {
                    return i;
                }
            }
            return -1;
        }
    }
}
