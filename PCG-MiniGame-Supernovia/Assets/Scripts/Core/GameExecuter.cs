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
        /// 如果返回人数少于preconception个数，则说明绑定失败
        /// </summary>
        /// <param name="gameState"></param>
        /// <param name="gameConfig"></param>
        /// <param name="eventCard"></param>
        /// <returns></returns>
        public static BindingInfo[] BindEventCharacters(GameState gameState, EventCard eventCard) {
            List<BindingInfo> bindingInfos = new List<BindingInfo>();
            foreach (var charPreconditon in eventCard.preconditonSet.characterPreconditions) {
                // 一个个preconception绑定过去
                var bindSuccess = false;
                if (charPreconditon.isRandom) {
                    for (int i = 0; i < gameState.characterDeck.Count; i++) {
                        // 不重复
                        if (!bindingInfos.Exists((e) => e.bindedCharacterIndex == i)) {
                            var binding = new BindingInfo();
                            binding.bindedCharacterIndex = i;
                            bindingInfos.Add(binding);

                            bindSuccess = true;
                            break;
                        }
                    }
                }
                else {
                    for (int i = 0; i < gameState.characterDeck.Count; i++) {
                        // 符合要求且不重复
                        if (!bindingInfos.Exists((e) => e.bindedCharacterIndex == i)
                            && gameState.characterDeck[i].HasTrait(charPreconditon.requiredTrait)) {
                            var binding = new BindingInfo();
                            binding.bindedCharacterIndex = i;
                            binding.bindedPersonalityIndex = gameState.characterDeck[i].FindPersonaltyIndex(charPreconditon.requiredTrait);
                            bindingInfos.Add(binding);

                            bindSuccess = true;
                            break;
                        }
                    }
                }

                if (!bindSuccess) {
                    break;
                }
            }
            return bindingInfos.ToArray();
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
                        //寻找一个贪婪的，转成廉洁
                        var corruptIndex = targetCharacter.FindPersonaltyIndex(Trait.corrupt);
                        if (corruptIndex >= 0) {
                            var modify = new GameStateModifyEvent(character, Trait.honest);
                            modify.AddTraitChangeConsequence(gameState, targetCharacter, corruptIndex, Trait.honest);
                            gameStateModifyEvents.Add(modify);
                            break;
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


        /// <summary>
        /// 因为cause有一样，所以consequence压到同一个event里面
        /// </summary>
        /// <param name="gameState"></param>
        /// <param name="gameConfig"></param>
        /// <param name="eventCard"></param>
        /// <param name="bindingInfos"></param>
        /// <returns></returns>
        public static GameStateModifyEvent CalculteEventConsequence(GameState gameState, GameConfig gameConfig, EventCard eventCard, BindingInfo[] bindingInfos) {
            var gameStateModifyEvent = new GameStateModifyEvent(eventCard,bindingInfos);
            if (eventCard.consequenceSet.characterConsequenceEnabled) {
                foreach (var characteConseq in eventCard.consequenceSet.characterConsequences) {
                    var bindingInfo = bindingInfos[characteConseq.bindFlag];
                    var bindedCharacer = gameState.characterDeck[bindingInfo.bindedCharacterIndex];
                    if (characteConseq.traitAlteration.type != TraitAlteration.Type.none) {
                        int personalityIndex;
                        Trait newTrait;
                        characteConseq.traitAlteration.CalculateAlteration(bindedCharacer, out personalityIndex, out newTrait);
                        // 角色，性格
                        gameStateModifyEvent.AddTraitChangeConsequence(gameState, bindedCharacer, personalityIndex, newTrait);
                    }
                    if (characteConseq.loyaltyAlteraion != 0) {      
                        // 角色, 忠诚
                        gameStateModifyEvent.AddLoyaltyChangeConsequence(gameState,bindedCharacer, characteConseq.loyaltyAlteraion);
                    }
                }
            }
            //  status 后果
            if (eventCard.consequenceSet.statusConsequenceEnabled) {
                gameStateModifyEvent.AddConsequence(eventCard.consequenceSet.statusConsequence.delta);
            }
            return gameStateModifyEvent;
        }



        // 本身应用的后果 ： 角色性格改变 - 先不设定！
        public static GameStateModifyEvent[] CalculteStratagemDecision(GameState gameState, GameConfig gameConfig, StratagemCard stratagem, CharacterCard stratagemProvider, bool isStratagemAccepted) {
            var gameStateModifyEvents = new List<GameStateModifyEvent>();
            if (isStratagemAccepted) {
                // 记次数
                gameState.acceptCountInCurrentRound++;

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
                    foreach (var character in gameState.characterDeck) {
                        if (character != stratagemProvider) {
                            var modify = new GameStateModifyEvent(stratagemProvider, Trait.arrogent);
                            modify.AddLoyaltyChangeConsequence(gameState,character,-1);
                            gameStateModifyEvents.Add(modify);
                            break;
                        }
                    }
                }

                // Trait-Pos:嫉妒
                if (gameState.acceptCountInCurrentRound >= gameConfig.jealousTraitThreshold) {
                    foreach (var character in gameState.characterDeck) {
                        if (character != stratagemProvider && character.HasTrait(Trait.jealous)) {
                            var modify = new GameStateModifyEvent(character, Trait.jealous);
                            modify.AddLoyaltyChangeConsequence(gameState, character, -1);
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
                        modify.AddLoyaltyChangeConsequence(gameState,stratagemProvider, 0);
                        gameStateModifyEvents.Add(modify);
                    }
                }
                else {
                    var modify = new GameStateModifyEvent( GameStateModifyCause.Type.madeStratagemDecision);
                    modify.AddLoyaltyChangeConsequence(gameState, stratagemProvider, -1);
                    gameStateModifyEvents.Add(modify);
                }

                // 本身应用的后果 ： 角色性格改变 - 先不设定！
                // stratagem.consequenceSet.traitAlterationWhenDecline.ApplyTo(stratagemProvider);
            }
            return gameStateModifyEvents.ToArray();
        }

        private static EventCard[] FiltEventCards(GameState gameState) {
            var qualifedEvents = new List<EventCard>();
            foreach (var e in gameState.eventDeck.ToArray()) {
                if (BindEventCharacters(gameState,e).Length == e.preconditonSet.characterPreconditions.Count) {
                    qualifedEvents.Add(e);
                }
            }
            return qualifedEvents.ToArray();
        }
    }
}