using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PCG;

[System.Serializable]
public class CharacterConsequence : Consequence{
    // TBD：（暂时的做法）
    // bindFlag就等于Precondition的下标
    public int bindFlag = 0;

    public TraitAlteration traitAlteration = new TraitAlteration();
    public int loyaltyAlteraion = 0;

    public string CreateDescription(BindingInfo bindingInfo,int indexInBindingSequence) {
        var characterShowupStr = string.Format("(c{0})", indexInBindingSequence.ToString());
        // 动画(t12怒),t是transfer，1 是bindinginfo，2是personalites的下标，2是tranfer的目标
        int indexOfTrait= 0;
        for (int i = 0; i < CharacterCard.PERSONALITY_COUNT; i++) {
            if (bindingInfo.bindedCharacter.personalities[i] == bindingInfo.bindedPersonalityOfCharacter) {
                indexOfTrait = i;
                break;
            }
        }
        var traitTansferStr = string.Format("(t{0}{1})", indexOfTrait.ToString(),(char)traitAlteration.targetTrait);
        var traitName = TraitUtils.TranslateToName(bindingInfo.bindedPersonalityOfCharacter.trait);
        var finalDescription = string.Format("{0}{1}变得{2}{3}", characterShowupStr, bindingInfo.bindedCharacter.name, traitTansferStr,traitName);

        if (loyaltyAlteraion < 0) {
            finalDescription += "且忠诚";
        }
        else {
            finalDescription += ", 并且不那么忠诚了";
        }
        return finalDescription;
    }
}
