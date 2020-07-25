using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsequenceApplier
{
    public static void  Apply(ConsequenceSet consequenceSet,CharacterCard[] bindedCharacters) {
        if (consequenceSet.fightConsequenceEnabled) {
            ApplyFightConsequence(consequenceSet.fightConsequence, bindedCharacters);
        }
    }

    private static void ApplyFightConsequence(FightConsequence fightConsequence, CharacterCard[] bindedCharacters) {
        if (bindedCharacters.Length < 2) {
            Debug.LogError("fight consequence， 角色绑定人数错误");
            return;
        }
        var attacker = fightConsequence.GetAttacker(bindedCharacters);
        var defender = fightConsequence.GetDefender(bindedCharacters);
        defender.attributes.HP -= attacker.attributes.atkVal;
    }
}
