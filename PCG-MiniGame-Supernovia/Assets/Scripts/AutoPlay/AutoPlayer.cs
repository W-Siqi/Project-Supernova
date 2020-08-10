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

        public SingleAutoPlayStatistic Play(GameState gameState, GameConfig gameConfig, bool recordLog = false) {
            Profiler.BeginSample("游玩- SingleAutoPlayStatistic Play()");
            var statistic = new SingleAutoPlayStatistic();
            if (recordLog) {
                statistic.gameLog = new GameLog();
            }

            int round;
            for (round = 0; round < gameConfig.roundCount; round++) {
                if (recordLog) {
                    statistic.gameLog.AddStageSwitchLog(true);
                }
                // 开局buff
                var roundStartModifies =  GameExecuter.CalculateBuffBeforeRound(gameState, gameConfig);
                GameStateModifyEvent.ApplyModificationsTo(gameState, roundStartModifies);
                // 记录日志
                if (recordLog) {
                    statistic.gameLog.AddLogs(roundStartModifies);
                }

                // council statge
                foreach (var character in gameState.characterDeck) {
                    if (character.HasTrait(Trait.silence)) {
                        if (Random.value < gameConfig.slicentTraitSlicenceProbility) {
                            // 生成一个空的modiy,主要是为了给回放系统
                            var silenceEvent = new GameStateModifyEvent(gameState,character, Trait.silence);
                            continue;
                        }
                    }
                    // 抽取决策卡
                    var straCard = gameState.GetStratagem(character,round);
                    // AI玩家做决定
                    var agreeDecision = Random.value < ProbailityOfAccept(straCard, character, gameState);

                    // 记录日志
                    if (recordLog) {
                        statistic.gameLog.AddLog(gameState.GetIndex(straCard), gameState.GetIndex(character), agreeDecision);
                    }

                    // 计算增量
                    var modifications = GameExecuter.CalculteStratagemDecision(gameState, gameConfig,straCard, character, agreeDecision);
                    // 应用增量
                    GameStateModifyEvent.ApplyModificationsTo(gameState, modifications);
                    // 记录日志
                    if (recordLog) {
                        statistic.gameLog.AddLogs(modifications);
                    }
                }

                // deathCheck
                if (GameExecuter.HasReachDeath(gameState)) {
                    statistic.lostInCouncil = true;
                    FillDeathReason(statistic, gameState);
                    break;
                }

                // eventstream阶段
                if (recordLog) {
                    statistic.gameLog.AddStageSwitchLog(false);
                }
                foreach (var selectedEvent in GameExecuter.SelectEventCards(gameState, gameConfig)) {
                    // 绑定
                    var bindingInfos = GameExecuter.BindEventCharacters(gameState, selectedEvent);
                    if (bindingInfos.Length < selectedEvent.preconditonSet.characterPreconditions.Count) {
                        // 绑定失败 - select之后又被改了
                        continue;
                    }

                    // 计算
                    var modification = GameExecuter.CalculteEventConsequence(gameState, gameConfig, selectedEvent, bindingInfos);
                    // 应用
                    GameStateModifyEvent.ApplyModificationsTo(gameState, new GameStateModifyEvent[] { modification });
                    // 记录日志
                    if (recordLog) {
                        statistic.gameLog.AddLog(modification);
                    }

                    if (GameExecuter.HasReachDeath(gameState)) {
                        break;
                    }
                }

                // deathCheck
                if (GameExecuter.HasReachDeath(gameState)) {
                    statistic.lostInEventStream = true;
                    FillDeathReason(statistic, gameState);
                }
            }

            if (recordLog) {
                statistic.gameLog.LogEnding(gameState);
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
