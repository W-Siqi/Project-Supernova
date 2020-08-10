using PCG;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PCGP {
    /// <summary>
    /// 必须和某一个gameState对应，因为里面的下标都是和gameState绑定的
    /// </summary>
    [System.Serializable]
    public class GameStateModifyEvent {
        // 专门对event的
        public BindingInfo[] bindingInfos = null;
        public GameStateModifyCause modifyCause = new GameStateModifyCause();
        public List<GameStateModifyConsequence> modifyConsequences = new List<GameStateModifyConsequence>();

        public static void ApplyModificationsTo(GameState gameState, GameStateModifyEvent[] gameStateModifyEvents) {
            foreach (var e in gameStateModifyEvents) {
                foreach (var conseq in e.modifyConsequences) {
                    conseq.ApplyTo(gameState);
                }
            }
        }

        public GameStateModifyEvent(GameStateModifyCause.Type causeType) {
            modifyCause.type = causeType;
        }

        /// <summary>
        /// 因触发角色性格
        /// </summary>
        /// <param name="character"></param>
        /// <param name="triggerdTrait"></param>
        public GameStateModifyEvent(CharacterCard character, Trait triggerdTrait) {
            modifyCause.type = GameStateModifyCause.Type.triggerTrait;
            modifyCause.trait = triggerdTrait;
            modifyCause.belongedCharacter = character;
        }

        /// <summary>
        /// 事件触发
        /// </summary>
        /// <param name="eventCard"></param>
        public GameStateModifyEvent(EventCard eventCard,BindingInfo[] bindingInfos) {
            modifyCause.type = GameStateModifyCause.Type.eventStream;
            modifyCause.eventCard = eventCard;
            this.bindingInfos = bindingInfos;
        }

        public void AddConsequence(StatusVector statusVector) {
            var newConsequence = new GameStateModifyConsequence();
            newConsequence.type = GameStateModifyConsequence.Type.valueChange;
            newConsequence.changeValue = new StatusVector(statusVector);
            modifyConsequences.Add(newConsequence);
        }

        // 结合gameState才有意义
        public void AddTraitChangeConsequence(GameState gameState, CharacterCard traitChangeCharacter,int personalityIndex, Trait newTrait) {
            var newConsequence = new GameStateModifyConsequence();
            newConsequence.type = GameStateModifyConsequence.Type.traitChange;
            newConsequence.traitChangeCharacterIndex = gameState.GetIndex(traitChangeCharacter);
            newConsequence.changedPersonalityIndex = personalityIndex;
            newConsequence.newTrait = newTrait;
            modifyConsequences.Add(newConsequence);
        }

        // 结合gameState才有意义
        public void AddLoyaltyChangeConsequence(GameState gameState, CharacterCard loyaltyChangeCharacter, int loyaltyDelta) {
            var newConsequence = new GameStateModifyConsequence();
            newConsequence.type = GameStateModifyConsequence.Type.loyaltyChange;
            newConsequence.loyaltyChangeCharacterIndex = gameState.GetIndex(loyaltyChangeCharacter);
            newConsequence.loyaltyDelta = loyaltyDelta;
            modifyConsequences.Add(newConsequence);
        }
    }
}
