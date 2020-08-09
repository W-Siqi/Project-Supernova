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

    // 动画(r[bindinfoIndex][trait-转ASCII]),r是remove trait
    // 动画(a[bindinfoIndex][trait-转ASCII]),a是add trait
    public string CreateDescription(BindingInfo bindingInfo,int indexInBindingSequence) {
        var characterShowupStr = string.Format("(c{0})", indexInBindingSequence.ToString());
        var traitAnimationStr = "";
        if (traitAlteration.type == TraitAlteration.Type.add) {
            traitAnimationStr = string.Format("(a{0}{1})", indexInBindingSequence.ToString(), (char)traitAlteration.targetTrait);
        }
        else if (traitAlteration.type == TraitAlteration.Type.remove) {
            traitAnimationStr = string.Format("(r{0}{1})", indexInBindingSequence.ToString(), (char)traitAlteration.targetTrait);
        }
        var finalDescription = string.Format("{0}{1}{2}发生了转变", characterShowupStr,traitAnimationStr, bindingInfo.bindedCharacter.name);

        if (loyaltyAlteraion > 0) {
            finalDescription += "且忠诚";
        }
        else if(loyaltyAlteraion < 0) {
            finalDescription += ", 并且不那么忠诚了";
        }
        return finalDescription;
    }
}
