using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterPrecondition : Precondition
{
    public List<Qualifier> qualifiers = new List<Qualifier>();

    public override bool SatisfiedByCurrentContext() {
           
        foreach (var character in StoryContext.instance.characterDeck) {
            bool characterSatisfied = true;
            foreach (var qualifier in qualifiers) {
                if (!character.qualifiers.Exists((i) => i.name == qualifier.name)) {
                    characterSatisfied = false;
                    break;
                }
            }

            // 但凡有一个满足，就出来
            if (characterSatisfied) {
                return true;
            }
        }

        return false;
    }
}
