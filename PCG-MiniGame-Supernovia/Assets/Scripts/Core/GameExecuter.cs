using PCG;
using PCGP;
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
        public static GameStateModifyEvent[] CalculateBuffBeforeRound(GameState gameState, GameConfig gameConfig) {
            var gameStateModifyEvents = new List<GameStateModifyEvent>();
            foreach (var character in gameState.characterDeck) {
                // Trait-Pos: 贪婪
                if (character.HasTrait(Trait.corrupt)) {
                    var modify = new GameStateModifyEvent(character, Trait.corrupt);
                    var addtionDelta = new StatusVector();
                    addtionDelta.money = -gameConfig.corrputTraitMoneyPerRound;
                    modify.AddConsequence(addtionDelta);
                    gameStateModifyEvents.Add(modify);
                }

                // Trait-Pos: 廉洁
                if (character.HasTrait(Trait.honest) && Random.value < gameConfig.honestProbability) {
                    foreach (var targetCharacter in gameState.characterDeck) {
                        foreach (var p in targetCharacter.personalities) {
                            if (p.trait == Trait.corrupt) {
                                var modify = new GameStateModifyEvent(character, Trait.honest);
                                modify.AddConsequence(targetCharacter, p, Trait.honest);
                                gameStateModifyEvents.Add(modify);
                                break;
                            }
                        }
                    }
                }
            }
            return gameStateModifyEvents.ToArray();
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


        // 本身应用的后果 ： 角色性格改变 - 先不设定！
        public static GameStateModifyEvent[] CalculteStratagemDecision(GameState gameStateToApply, GameConfig gameConfig, StratagemCard stratagem, CharacterCard stratagemProvider, bool isStratagemAccepted) {
            var gameStateModifyEvents = new List<GameStateModifyEvent>();
            if (isStratagemAccepted) {
                // 记次数
                gameStateToApply.acceptCountInCurrentRound++;

                // 本身应用的后果
                var origanlDelta = new StatusVector(stratagem.consequenceSet.statusConsequenceWhenAccept.delta);
                var stratagemCausedModify = new GameStateModifyEvent(GameStateModifyCause.Type.madeStratagemDecision);
                stratagemCausedModify.AddConsequence(origanlDelta);
                // 本身应用的后果 ： 角色性格改变 - 先不设定！
                // stratagem.consequenceSet.traitAlterationWhenAccept.ApplyTo(stratagemProvider);
                gameStateModifyEvents.Add(stratagemCausedModify);

                // Trait-Pos:奸诈
                if (stratagemProvider.HasTrait(Trait.tricky)) {
                    var modify = new GameStateModifyEvent(stratagemProvider, Trait.tricky);
                    modify.AddConsequence( -2* origanlDelta);
                    gameStateModifyEvents.Add(modify);
                }
                // Trait-Pos:明智
                if (stratagemProvider.HasTrait(Trait.wise)) {
                    var modify = new GameStateModifyEvent(stratagemProvider, Trait.wise);
                    var ampifiedDelta = new StatusVector(origanlDelta);
                    ampifiedDelta.AmplifyValueIfPositive(gameConfig.wiseTraitAmplifyRate);
                    modify.AddConsequence(ampifiedDelta-origanlDelta);
                    gameStateModifyEvents.Add(modify);
                }

                // Trait-Pos: 好战
                if (stratagemProvider.HasTrait(Trait.warlike)) {
                    var modify = new GameStateModifyEvent(stratagemProvider, Trait.warlike);
                    var addtionDelta = new StatusVector();
                    addtionDelta.army = gameConfig.warlikeTraitArmyValueWhenAccept;
                    modify.AddConsequence(addtionDelta);
                    gameStateModifyEvents.Add(modify);
                }

                // Trait-Pos:残暴
                if (stratagemProvider.HasTrait(Trait.cruel)) {
                    var modify = new GameStateModifyEvent(stratagemProvider, Trait.cruel);
                    var addtionDelta = new StatusVector();
                    addtionDelta.people = - gameConfig.cruelTraitPeopleValuePerDecision;
                    modify.AddConsequence(addtionDelta);
                    gameStateModifyEvents.Add(modify);
                }

                // Trait-Pos:傲慢
                if (stratagemProvider.HasTrait(Trait.arrogent)) {
                    foreach (var character in gameStateToApply.characterDeck) {
                        if (character != stratagemProvider) {
                            var modify = new GameStateModifyEvent(stratagemProvider, Trait.arrogent);
                            modify.AddConsequence(character,-1);
                            gameStateModifyEvents.Add(modify);
                            break;
                        }
                    }
                }

                // Trait-Pos:嫉妒
                if (gameStateToApply.acceptCountInCurrentRound >= gameConfig.jealousTraitThreshold) {
                    foreach (var character in gameStateToApply.characterDeck) {
                        if (character != stratagemProvider && character.HasTrait(Trait.jealous)) {
                            var modify = new GameStateModifyEvent(character, Trait.jealous);
                            modify.AddConsequence(character, -1);
                            gameStateModifyEvents.Add(modify);
                        }
                    }
                }
            }
            else {
                // Trait-Pos:宽容
                if (stratagemProvider.HasTrait(Trait.tolerant)) {
                    if (Random.value > gameConfig.tolerantTraitKeepLoyaltyProbability) {
                        var modify = new GameStateModifyEvent(stratagemProvider, Trait.tolerant);
                        modify.AddConsequence(stratagemProvider, 0);
                        gameStateModifyEvents.Add(modify);
                    }
                }
                else {
                    stratagemProvider.loyalty -= 1;
                }

                // 本身应用的后果 ： 角色性格改变 - 先不设定！
                // stratagem.consequenceSet.traitAlterationWhenDecline.ApplyTo(stratagemProvider);
            }
            return gameStateModifyEvents.ToArray();
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