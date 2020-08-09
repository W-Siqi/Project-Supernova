using PCG;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PCGP {
    public class GameStateModifyEvent {
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

        public void AddConsequence(StatusVector statusVector) {
            var newConsequence = new GameStateModifyConsequence();
            newConsequence.type = GameStateModifyConsequence.Type.valueChange;
            newConsequence.changeValue = new StatusVector(statusVector);
            modifyConsequences.Add(newConsequence);
        }

        public void AddConsequence(CharacterCard traitChangeCharacter,Personality changedPersonality, Trait newTrait) {
            var newConsequence = new GameStateModifyConsequence();
            newConsequence.type = GameStateModifyConsequence.Type.traitChange;
            newConsequence.traitChangeCharacter = traitChangeCharacter;
            newConsequence.changedPersonalityIndex = traitChangeCharacter.FindPersonaltyIndex(changedPersonality);
            newConsequence.newTrait = newTrait;
            modifyConsequences.Add(newConsequence);
        }

        public void AddConsequence(CharacterCard loyaltyChangeCharacter, int loyaltyDelta) {
            var newConsequence = new GameStateModifyConsequence();
            newConsequence.type = GameStateModifyConsequence.Type.loyaltyChange;
            newConsequence.loyaltyChangeCharacter = loyaltyChangeCharacter;
            newConsequence.loyaltyDelta = loyaltyDelta;
            modifyConsequences.Add(newConsequence);
        }
    }
}
