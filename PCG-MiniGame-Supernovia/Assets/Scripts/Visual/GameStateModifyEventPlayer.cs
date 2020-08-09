using PCGP;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PCG {
    public class GameStateModifyEventPlayer : MonoBehaviour {
        [SerializeField]
        private TraitTriggeredCauseViewer traitTriggeredCauseViewer;

        public IEnumerator PlayEvents(GameStateModifyEvent[] gameStateModifyEvents) {
            foreach (var e in gameStateModifyEvents) {
                yield return StartCoroutine(PlayGameStateModifyEvent(e));
            }
        }

        public IEnumerator PlayGameStateModifyEvent(GameStateModifyEvent gameStateModifyEvent) {
            // 显示 cause
            if (gameStateModifyEvent.modifyCause.type == GameStateModifyCause.Type.triggerTrait) {
                traitTriggeredCauseViewer.ViewCause(gameStateModifyEvent.modifyCause);
                yield return new WaitForSeconds(1f);
            }
            else if (gameStateModifyEvent.modifyCause.type == GameStateModifyCause.Type.madeStratagemDecision) { 
                // 决策的casue没必要显示
            }

            // 轮流播放结果
            foreach (var conseq in gameStateModifyEvent.modifyConsequences) {
                yield return PlayModifyConsequence(gameStateModifyEvent.modifyCause, conseq);
            }

            // 结束显示 cause
            if (gameStateModifyEvent.modifyCause.type == GameStateModifyCause.Type.triggerTrait) {
                traitTriggeredCauseViewer.EndViewCause();
                yield return new WaitForSeconds(2f);
            }
        }

        // 显示结果
        private IEnumerator PlayModifyConsequence(GameStateModifyCause cause, GameStateModifyConsequence consequence) {
            var characterPannel = ViewManager.instance.characterStausPannel;
            var statusVectorPannel = ViewManager.instance.statusVectorPannel;

            if (consequence.type == GameStateModifyConsequence.Type.loyaltyChange) {
                yield return StartCoroutine(characterPannel.ViewLoyaltyChange(
                    consequence.loyaltyChangeCharacter,
                    consequence.loyaltyDelta
                    ));
            }
            else if (consequence.type == GameStateModifyConsequence.Type.traitChange) {
                yield return StartCoroutine(characterPannel.ViewTraitChange(
                    consequence.traitChangeCharacter,
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

