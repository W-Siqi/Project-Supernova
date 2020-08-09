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

    public CharacterCard traitChangeCharacter;
    public int changedPersonalityIndex;
    public Trait newTrait;

    public CharacterCard loyaltyChangeCharacter;
    public int loyaltyDelta;

    public void ApplyTo(GameState gameState) {
        if (type == Type.valueChange) {
            gameState.statusVector += changeValue;
        }
        else if (type == Type.traitChange) {
            traitChangeCharacter.personalities[changedPersonalityIndex].trait = newTrait;
        }
        else if (type == Type.loyaltyChange) {
            loyaltyChangeCharacter.loyalty += loyaltyDelta;
        }
    }
}
