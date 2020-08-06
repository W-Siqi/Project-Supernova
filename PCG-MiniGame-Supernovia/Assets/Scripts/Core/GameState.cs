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
    }
}
