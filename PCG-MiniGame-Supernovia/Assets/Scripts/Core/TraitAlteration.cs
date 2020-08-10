using PCG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TraitAlteration {
    public enum Type {none, add, remove}

    public Trait targetTrait;
    public Type type;

    public TraitAlteration() {
        targetTrait = Trait.none;
        type = Type.none;
    }

    public void CalculateAlteration(CharacterCard target, out int changedPersonalityIndex, out Trait newTrait) {
        changedPersonalityIndex = 0;
        if (type == Type.add) {
            newTrait = targetTrait;
            // 挑空的
            bool existEmpty = false;
            for (int i = 0; i < target.personalities.Length; i++) {
                if (target.personalities[i].trait == Trait.none) {
                    existEmpty = true;
                    changedPersonalityIndex = i;
                    break;
                }
            }

            // 没空的add就随机覆盖了
            if (!existEmpty) {
                changedPersonalityIndex = Random.Range(0, CharacterCard.PERSONALITY_COUNT);
            }
        }
        else if (type == Type.remove) {
            newTrait = Trait.none;
            for (int i = 0; i < target.personalities.Length; i++) {
                if (target.personalities[i].trait == targetTrait) {
                    changedPersonalityIndex = i;
                }
            }
        }
        else {
            throw new System.Exception("unf");
        }
    }

    //public void ApplyTo(CharacterCard target) {
    //    if (type == Type.add) {
    //        bool existEmpty = false;
    //        foreach (var personality in target.personalities) {
    //            if (personality.trait == Trait.none) {
    //                existEmpty = true;
    //                personality.trait = targetTrait;
    //            }
    //        }

    //        if (!existEmpty) {
    //            var selectedPersonality = target.personalities[Random.Range(0, CharacterCard.PERSONALITY_COUNT)];
    //            selectedPersonality.trait = targetTrait;
    //        }
    //    }
    //    else if (type == Type.remove) {
    //        foreach (var personality in target.personalities) {
    //            if (personality.trait == targetTrait) {
    //                personality.trait = Trait.none;
    //            }
    //        }
    //    }
    //}
}
