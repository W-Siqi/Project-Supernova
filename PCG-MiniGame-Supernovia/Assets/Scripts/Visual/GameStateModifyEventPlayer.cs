using PCGP;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PCG {
    public class GameStateModifyEventPlayer : MonoBehaviour {
        [SerializeField]
        private TraitTriggeredCauseViewer traitTriggeredCauseViewer;

        // 用来优化节奏的
        private GameStateModifyCause previousCause = null;
        public IEnumerator PlayEvents(GameState gameState, GameStateModifyEvent[] gameStateModifyEvents) {
            previousCause = null;
            foreach (var e in gameStateModifyEvents) {
                yield return StartCoroutine(PlayEvent(gameState, e));
            }
        }

        public IEnumerator PlayEvent(GameState gameState, GameStateModifyEvent gameStateModifyEvent) {
            if (gameStateModifyEvent.modifyCause.type == GameStateModifyCause.Type.eventStream) {
                yield return StartCoroutine(PlayEventStreamStageModification(gameState, gameStateModifyEvent));
            }
            else {
                yield return StartCoroutine(PlayCouncilStageModification(gameState, gameStateModifyEvent));
            }
            previousCause = gameStateModifyEvent.modifyCause;
        }

        private IEnumerator PlayCouncilStageModification(GameState gameState, GameStateModifyEvent gameStateModifyEvent) {
            var samePreviousCharacter = false;
            if (previousCause != null
                && previousCause.type == GameStateModifyCause.Type.triggerTrait
                && previousCause.belongedCharacterIndex == gameStateModifyEvent.modifyCause.belongedCharacterIndex) {
                samePreviousCharacter = true;
            }

            // 显示 cause
            if (gameStateModifyEvent.modifyCause.type == GameStateModifyCause.Type.triggerTrait) {
                if (!samePreviousCharacter) {
                    yield return new WaitForSeconds(1f);
                }
                traitTriggeredCauseViewer.ViewCause(gameState, gameStateModifyEvent.modifyCause,samePreviousCharacter);
                yield return new WaitForSeconds(2f);
            }
            else if (gameStateModifyEvent.modifyCause.type == GameStateModifyCause.Type.madeStratagemDecision) {
                // 决策的casue没必要显示
            }

            if (gameStateModifyEvent.modifyConsequences.Count == 0
                && gameStateModifyEvent.modifyCause.type == GameStateModifyCause.Type.triggerTrait
                && gameStateModifyEvent.modifyCause.trait == Trait.silence) {
                // silence 的特殊的沉默效果
                var character = gameState.characterDeck[gameStateModifyEvent.modifyCause.belongedCharacterIndex];
                ViewManager.instance.characterStausPannel.OnSelect(character);
                ViewManager.instance.characterStausPannel.ViewSentance(character, "我今日无要事商议");
                yield return new WaitForSeconds(2f);
            }
            // 其他类型轮流播放结果
            foreach (var conseq in gameStateModifyEvent.modifyConsequences) {
                yield return PlayModifyConsequence(gameState, gameStateModifyEvent.modifyCause, conseq);
            }         

            // 结束显示 cause
            if (gameStateModifyEvent.modifyCause.type == GameStateModifyCause.Type.triggerTrait) {
                traitTriggeredCauseViewer.EndViewCause();
            }
        }


        private IEnumerator PlayEventStreamStageModification(GameState gameState, GameStateModifyEvent gameStateModifyEvent) {
            var eventCard = gameState.eventDeck[gameStateModifyEvent.modifyCause.eventCardIndex];
            var description = EventDescription.Generate(gameState, eventCard, gameStateModifyEvent.bindingInfos);    
            yield return StartCoroutine(ViewManager.instance.eventDescriptionPlayer.PlayEventDescription(gameState, description));
            // 暂时的做法，status改变直接走countil一样
            foreach (var coneq in gameStateModifyEvent.modifyConsequences) {
                // Debug.Log("[event conseq的type] " + coneq.type);
                if (coneq.type == GameStateModifyConsequence.Type.valueChange) {
                    yield return StartCoroutine( ViewManager.instance.statusVectorPannel.ViewStatusVectorChange(coneq.changeValue));
                }
            }
            yield return new WaitForSeconds(1f);
        }


        // 显示结果
        private IEnumerator PlayModifyConsequence(GameState gameState, GameStateModifyCause cause, GameStateModifyConsequence consequence) {
            var characterPannel = ViewManager.instance.characterStausPannel;
            var statusVectorPannel = ViewManager.instance.statusVectorPannel;

            if (consequence.type == GameStateModifyConsequence.Type.loyaltyChange) {
                yield return StartCoroutine(characterPannel.ViewLoyaltyChange(
                    gameState.characterDeck[consequence.loyaltyChangeCharacterIndex],
                    consequence.loyaltyDelta
                    ));
            }
            else if (consequence.type == GameStateModifyConsequence.Type.traitChange) {
                yield return StartCoroutine(characterPannel.ViewTraitChange(
                    gameState.characterDeck[consequence.traitChangeCharacterIndex],
                    consequence.changedPersonalityIndex,
                    consequence.newTrait ));
            }
            else if (consequence.type == GameStateModifyConsequence.Type.valueChange) {
                if (cause.type == GameStateModifyCause.Type.triggerTrait) {
                    yield return StartCoroutine(statusVectorPannel.ViewStatusVectorChange(consequence.changeValue, cause.trait));
                }
                else {
                    yield return StartCoroutine(statusVectorPannel.ViewStatusVectorChange(consequence.changeValue));
                }
            }
        }
    }
}

