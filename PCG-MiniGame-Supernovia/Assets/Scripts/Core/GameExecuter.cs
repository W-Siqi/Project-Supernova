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
                    var modify = new GameStateModifyEvent(gameState, 
                        character, 
                        Trait.corrupt,
                        string.Format("贪婪的[{0}]每回合都会贪金币", character.name));

                    var addtionDelta = new StatusVector();
                    addtionDelta.money = -gameConfig.corrputTraitMoneyPerRound;
                    modify.AddConsequence(addtionDelta);
                    gameStateModifyEvents.Add(modify);
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
            var gameStateModifyEvent = new GameStateModifyEvent(gameState, eventCard, bindingInfos);
            if (eventCard.consequenceSet.characterConsequenceEnabled) {
                foreach (var characteConseq in eventCard.consequenceSet.characterConsequences) {
                    if (characteConseq.bindFlag >= bindingInfos.Length) {
                        Debug.LogError(eventCard.name + "  实际bindinfo数: " + bindingInfos.Length + "bindingflag：" + characteConseq.bindFlag);
                    }
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


                // Trait-Pos: 廉洁
                if (stratagemProvider.HasTrait(Trait.honest) && Random.value < gameConfig.honestProbabilityPerStratagem) {
                    foreach (var targetCharacter in gameState.characterDeck) {
                        //寻找一个贪婪的，转成廉洁
                        var corruptIndex = targetCharacter.FindPersonaltyIndex(Trait.corrupt);
                        if (corruptIndex >= 0) {
                            var modify = new GameStateModifyEvent(gameState, 
                                stratagemProvider, 
                                Trait.honest,
                                string.Format("采纳了廉洁[{0}]的建议，\n 消除了[{1}]的腐败", stratagemProvider.name,targetCharacter.name));

                            modify.AddTraitChangeConsequence(gameState, targetCharacter, corruptIndex, Trait.honest);
                            gameStateModifyEvents.Add(modify);
                            break;
                        }
                    }
                }

                // Trait-Pos:奸诈
                if (stratagemProvider.HasTrait(Trait.tricky) && Random.value < gameConfig.trickyTraitTriggerProb) {
                    var modify = new GameStateModifyEvent(gameState, 
                        stratagemProvider,
                        Trait.tricky,
                        string.Format("采纳了谎话连篇[{0}]的建议，事与愿违", stratagemProvider.name));

                    modify.AddConsequence( -2* origanlDelta);
                    gameStateModifyEvents.Add(modify);
                }
                // Trait-Pos:明智
                if (stratagemProvider.HasTrait(Trait.wise)) {
                    var modify = new GameStateModifyEvent(gameState, 
                        stratagemProvider, 
                        Trait.wise,
                        string.Format("采纳了智慧[{0}]的建议，提升了收益", stratagemProvider.name));

                    var ampifiedDelta = new StatusVector(origanlDelta);
                    ampifiedDelta.AmplifyValueIfPositive(gameConfig.wiseTraitAmplifyRate);
                    modify.AddConsequence(ampifiedDelta-origanlDelta);
                    gameStateModifyEvents.Add(modify);
                }

                // Trait-Pos: 好战
                if (stratagemProvider.HasTrait(Trait.warlike)) {
                    var modify = new GameStateModifyEvent(gameState, 
                        stratagemProvider, 
                        Trait.warlike,
                        string.Format("采纳了勇猛[{0}]的建议，提升了军队",stratagemProvider.name));

                    var addtionDelta = new StatusVector();
                    addtionDelta.army = gameConfig.warlikeTraitArmyValueWhenAccept;
                    modify.AddConsequence(addtionDelta);
                    gameStateModifyEvents.Add(modify);
                }

                // Trait-Pos:残暴
                if (stratagemProvider.HasTrait(Trait.cruel)) {
                    var modify = new GameStateModifyEvent(gameState, 
                        stratagemProvider, 
                        Trait.cruel,
                        string.Format("采纳了残暴的[{0}]的建议,民心下降", stratagemProvider.name));

                    var addtionDelta = new StatusVector();
                    addtionDelta.people = - gameConfig.cruelTraitPeopleValuePerDecision;
                    modify.AddConsequence(addtionDelta);
                    gameStateModifyEvents.Add(modify);

                    // Trait-Pos：宽容 - 联动
                    foreach (var character in gameState.characterDeck) {
                        if (character.HasTrait(Trait.tolerant) && Random.value < gameConfig.tolerantWipeCruelProb) {
                            var tolerantModify = new GameStateModifyEvent(gameState, 
                                character, 
                                Trait.tolerant,
                                string.Format("[{0}]的宽容，\n 降低了[{1}]的残暴对名的影响", character.name,stratagemProvider.name));

                            var tolerAddtionDelta = new StatusVector();
                            tolerAddtionDelta.people = gameConfig.cruelTraitPeopleValuePerDecision;
                            tolerantModify.AddConsequence(addtionDelta);
                            gameStateModifyEvents.Add(tolerantModify);
                            break;
                        }
                    }
                }

                // Trait-Pos:傲慢 - 联动
                if (stratagemProvider.HasTrait(Trait.arrogent)) {
                    foreach (var character in gameState.characterDeck) {
                        if (character != stratagemProvider && character.HasTrait( Trait.arrogent)) {
                            var modify = new GameStateModifyEvent(gameState,
                                character, 
                                Trait.arrogent,
                                string.Format("[{0}]和[{1}]同为傲慢之人，无论采纳谁的建议，另一个人都会忠诚度-1", stratagemProvider.name, character.name));

                            modify.AddLoyaltyChangeConsequence(gameState,character,-1);
                            gameStateModifyEvents.Add(modify);
                            break;
                        }
                    }
                }

                // Trait-Pos:嫉妒 - 联动
                if (gameState.acceptCountInCurrentRound >= gameConfig.jealousTraitThreshold) {
                    foreach (var character in gameState.characterDeck) {
                        if (character != stratagemProvider && character.HasTrait(Trait.jealous)) {
                            var modify = new GameStateModifyEvent(gameState, 
                                character,
                                Trait.jealous,
                                string.Format("你采纳了3个以上的建议，[{0}]的嫉妒使他忠诚度减少", character.name));

                            modify.AddLoyaltyChangeConsequence(gameState, character, -1);
                            gameStateModifyEvents.Add(modify);
                        }
                    }
                }

                // Trait-Pos： 贪婪 - 联动
                if (origanlDelta.money > 0) {
                    foreach (var character in gameState.characterDeck) {
                        if (character.HasTrait(Trait.corrupt)) {
                            var modify = new GameStateModifyEvent(gameState, 
                                character, 
                                Trait.corrupt
                                , string.Format("由于[{0}]的腐败,\n 政策【{1}】 财政收益减少", stratagemProvider.name, stratagem.name));

                            var addtionDelta = new StatusVector();
                            addtionDelta.money = -gameConfig.corrputTraitMoneyPerStratagem;
                            modify.AddConsequence(addtionDelta);
                            gameStateModifyEvents.Add(modify);
                        }
                    }
                }
            }
            else {
                if (stratagemProvider.HasTrait(Trait.tolerant)) {
                    // Trait-Pos:宽容
                    if (Random.value > gameConfig.tolerantTraitKeepLoyaltyProbability) {
                        var modify = new GameStateModifyEvent(gameState, 
                            stratagemProvider, 
                            Trait.tolerant,
                            string.Format("由于[{0}]的宽容，没采纳建议也不会减忠诚度", stratagemProvider.name));

                        modify.AddLoyaltyChangeConsequence(gameState,stratagemProvider, 0);
                        gameStateModifyEvents.Add(modify);
                    }
                }
                else {
                    var modify = new GameStateModifyEvent( GameStateModifyCause.Type.madeStratagemDecision);
                    modify.AddLoyaltyChangeConsequence(gameState, stratagemProvider, -1);
                    gameStateModifyEvents.Add(modify);
                }


                // Trait-Pos： 好战 - 不采纳
                if (stratagem.consequenceSet.statusConsequenceWhenAccept.delta.army > 0) {
                    foreach (var character in gameState.characterDeck) {
                        if (character.HasTrait(Trait.warlike)) {
                            var modify = new GameStateModifyEvent(gameState, 
                                character, 
                                Trait.warlike,
                                string.Format("增加军力的政策【{0}】未采纳,\n勇猛[{1}]的忠诚降低", stratagem.name, character.name));

                            modify.AddLoyaltyChangeConsequence(gameState, character, -1);
                            gameStateModifyEvents.Add(modify);
                        }
                    }
                }

                // Trait-Pos： 沉默 - 不采纳
                if (stratagem.consequenceSet.statusConsequenceWhenAccept.delta.people > 0) {
                    foreach (var character in gameState.characterDeck) {
                        if (character.HasTrait(Trait.silence)) {
                            var modify = new GameStateModifyEvent(gameState, 
                                character, 
                                Trait.silence,
                                string.Format("增加民心的政策【{0}】未采纳,\n沉默的[{1}]也看不下去了", stratagem.name, character.name));

                            modify.AddLoyaltyChangeConsequence(gameState, character, -1);
                            gameStateModifyEvents.Add(modify);
                        }
                    }
                }
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