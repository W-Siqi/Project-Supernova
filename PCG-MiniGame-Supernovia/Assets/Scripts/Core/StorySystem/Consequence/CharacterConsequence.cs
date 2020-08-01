using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PCG;

[System.Serializable]
public class CharacterConsequence : Consequence{
    // TBD：（暂时的做法）
    // bindFlag就等于Precondition的下标
    public int bindFlag = 0;
    // 想要修改成的特质方向
    public Trait traitToBecome;
    // 超这个新人格改变的强度（如果强度很低，而目标人格和当前人格相距很远的话，不一定能抵达目标人格）
    public int transferStrength = 1;

    public BindingInfo BindFromSequence(BindingInfo[] bindingInfoSequence) {
        return bindingInfoSequence[bindFlag];
    }
    //public CharacterCard GetBindedCharacter(CharacterCard[] bindedCharacters) {
    //    return bindedCharacters[bindFlag - 1];
    //}
}
