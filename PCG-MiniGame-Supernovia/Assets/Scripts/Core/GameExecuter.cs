using PCG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PCG {
    public class GameExecuter {
        public static EventCard[] SelectEventCards(GameState gameState,GameConfig gameConfig) {
            var qualifeiedEvents = FiltEventCards(gameState);
            var seleced = new List<EventCard>();
            for (int i = 0; i < gameConfig.eventCountPerRound; i++) {
                var selectedEvent = qualifeiedEvents[Random.Range(0, qualifeiedEvents.Length)];
                seleced.Add(selectedEvent);
            }
            return seleced.ToArray();
        }

        /// <summary>
        /// 主要是清算人物性格
        /// </summary>
        public static void ApplyBuffBeforeRound(GameState gameState, GameConfig gameConfig, bool viusalEffectOn = false) {
            foreach (var character in gameState.characterDeck) {
                // Trait-Pos: 贪婪
                if (character.HasTrait(Trait.corrupt)) {
                    var corrputValue = gameConfig.corrputTraitMoneyPerRound;
                    gameState.statusVector.money -= corrputValue;

                    if (viusalEffectOn) {
                        ViewManager.instance.characterStausPannel.HightLightCharacterTrait(character, Trait.corrupt);
                    }
                }
            }
        }

        public static bool HasReachDeath(GameState checkTarget) {
            var status = checkTarget.statusVector;
            if (status.army < 0 || status.money < 0 || status.people < 0) {
                return true;
            }
            foreach (var character in checkTarget.characterDeck) {
                if (character.loyalty <= 0) {
                    return true;
                }
            }
            return false;
        }

        private static EventCard[] FiltEventCards(GameState gameState) {
            var qualifedEvents = new List<EventCard>();
            foreach (var e in gameState.eventDeck.ToArray()) {
                if (e.preconditonSet.SatisfiedAt(gameState)) {
                    qualifedEvents.Add(e);
                }
            }
            return qualifedEvents.ToArray();
        }
    }
}