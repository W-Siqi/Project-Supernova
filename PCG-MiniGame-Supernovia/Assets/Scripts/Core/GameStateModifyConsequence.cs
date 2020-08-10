using PCG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameStateModifyConsequence 
{
    public enum Type {
        valueChange, traitChange, loyaltyChange
    }

    public Type type;

    public StatusVector changeValue;

    public int traitChangeCharacterIndex;
    public int changedPersonalityIndex;
    public Trait newTrait;

    public int loyaltyChangeCharacterIndex;
    public int loyaltyDelta;

    public void ApplyTo(GameState gameState) {
        if (type == Type.valueChange) {
            gameState.statusVector += changeValue;
        }
        else if (type == Type.traitChange) {
            var traitChangeCharacter = gameState.characterDeck[traitChangeCharacterIndex];
            traitChangeCharacter.personalities[changedPersonalityIndex].trait = newTrait;
        }
        else if (type == Type.loyaltyChange) {
            var loyaltyChangeCharacter = gameState.characterDeck[loyaltyChangeCharacterIndex];
            loyaltyChangeCharacter.loyalty += loyaltyDelta;
        }
    }
}
