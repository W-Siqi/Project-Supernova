using PCGP;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

namespace PCG {
    public abstract class AutoPlayer {
        /// <summary>
        /// 返回0-1
        /// </summary>
        /// <param name="stratagemCard"></param>
        /// <param name="gameState"></param>
        /// <returns></returns>
        protected abstract float ProbailityOfAccept(StratagemCard stratagemCard,CharacterCard provider, GameState gameState);

        public SingleAutoPlayStatistic Play(GameState gameState, GameConfig gameConfig) {
            Profiler.BeginSample("游玩- SingleAutoPlayStatistic Play()");
            var statistic = new SingleAutoPlayStatistic();
            int round;
            for (round = 0; round < gameConfig.roundCount; round++) {
                // buff
                var modifies =  GameExecuter.CalculateBuffBeforeRound(gameState, gameConfig);
                GameStateModifyEvent.ApplyModificationsTo(gameState, modifies);

                // council
                foreach (var character in gameState.characterDeck) {
                    if (character.HasTrait(Trait.silence)) {
                        if (Random.value < gameConfig.slicentTraitSlicenceProbility) {
                            continue;
                        }
                    }
                    var straCard = gameState.stratagemDict[character][round];
                    var agreeDecision = Random.value < ProbailityOfAccept(straCard,character,gameState);
                    straCard.consequenceSet.Apply(gameState, gameConfig, character, agreeDecision);
                }

                // deathCheck
                if (GameExecuter.HasReachDeath(gameState)) {
                    statistic.lostInCouncil = true;
                    FillDeathReason(statistic, gameState);
                    break;
                }

                // eventstream
                foreach (var selectedEvent in GameExecuter.SelectEventCards(gameState, gameConfig)) {
                    var bindingInfos = selectedEvent.preconditonSet.Bind(PlayData.instance.gameState);
                    selectedEvent.consequenceSet.Apply(bindingInfos, gameState);
                }

                // deathCheck
                if (GameExecuter.HasReachDeath(gameState)) {
                    statistic.lostInEventStream = true;
                    FillDeathReason(statistic, gameState);
                }
            }

            statistic.win = !GameExecuter.HasReachDeath(gameState);
            statistic.roundsSurvive = round;
            Profiler.EndSample();
            return statistic;
        }


        private static void FillDeathReason(SingleAutoPlayStatistic statisTofillResult,GameState gameState) {
            var status = gameState.statusVector;
            if (status.army < 0) {
                statisTofillResult.lostForArmy = true;
            }
            if (status.money < 0) {
                statisTofillResult.lostForMoney = true;
            }
            if (status.people < 0) {
                statisTofillResult.lostForPeople = true;
            }

            foreach (var character in gameState.characterDeck) {
                if (character.loyalty <= 0) {
                    statisTofillResult.lostForloyalty = true;
                }
            }
        }
    }
}
