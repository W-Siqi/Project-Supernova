using PCG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TraitAlteration {
    public enum Type {none, add, remove }

    public Trait targetTrait;
    public Type type;

    public TraitAlteration() {
        targetTrait = Trait.none;
        type = Type.none;
    }

    public void ApplyTo(CharacterCard target) {
        if (type == Type.add) {
            bool existEmpty = false;
            foreach (var personality in target.personalities) {
                if (personality.trait ==  Trait.none) {
                    existEmpty = true;
                    personality.trait = targetTrait;
                }
            }

            if (!existEmpty) {
                var selectedPersonality = target.personalities[Random.Range(0, CharacterCard.PERSONALITY_COUNT)];
                selectedPersonality.trait = targetTrait;
            }
        }
        else if (type == Type.remove) {
            foreach (var personality in target.personalities) {
                if (personality.trait == targetTrait) {
                    personality.trait = Trait.none;
                }
            }
        }
    }
}
